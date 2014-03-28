using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NLog;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using SimpleScale.Common;

namespace SimpleScale.Queues
{
    public class ServiceBusQueueManager<InputT, ResultU> : IQueueManager<InputT, ResultU> 
    {
        private readonly NamespaceManager _namespaceManager;
        private readonly MessagingFactory _messagingFactory;
        private readonly QueueClient _workQueueClient;
        private readonly QueueClient _workCompletedQueueClient;
        private readonly string _workQueueName;

        public ServiceBusQueueManager(string connectionString, string workQueueName, string workCompletedQueueName)
        {
            _namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            _messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);

            _workQueueClient = CreateQueueClient(workQueueName);
            _workCompletedQueueClient = CreateQueueClient(workCompletedQueueName);

            _workQueueName = workQueueName;
        }

        public void AddJobs(List<Job<InputT>> jobs)
        {
            var messagesList = jobs.Select(job => new BrokeredMessage(job));
            _workQueueClient.SendBatch(messagesList);
        }

        public bool ReadJobAndDoWork(Func<Job<InputT>, ResultU> doWork, out Job<InputT> job, out ResultU result)
        {
            job = null;
            result = default(ResultU);
            var message = _workQueueClient.Receive(new TimeSpan(0, 0, 0, 2));
            if (message == null)
                return false;
            job = message.GetBody<Job<InputT>>();
            result = doWork(job);
            _workQueueClient.Complete(message.LockToken);
            return true;
        }

        public void AddCompleteJob(Result<ResultU> result)
        {
            _workCompletedQueueClient.Send(new BrokeredMessage(result));
        }

        public bool ReadCompletedJob(out Result<ResultU> result)
        {
            result = null;
            var message = _workCompletedQueueClient.Receive(new TimeSpan(0, 0, 0, 2));
            if (message == null)
                return false;
            result = message.GetBody<Result<ResultU>>();
            _workCompletedQueueClient.Complete(message.LockToken);
            return true;
        }
        
        public void Dispose()
        {
            _messagingFactory.Close();
        }

        private QueueClient CreateQueueClient(string queueName)
        {
            CreateQueue(queueName);
            return _messagingFactory.CreateQueueClient(queueName, ReceiveMode.PeekLock);
        }

        private void CreateQueue(string queueName)
        {
            if (!_namespaceManager.QueueExists(queueName))
                _namespaceManager.CreateQueue(queueName);
        }

        private QueueDescription GetQueue(string queueName)
        {
            CreateQueue(queueName);
            return _namespaceManager.GetQueue(queueName);
        }

        public IEnumerable<BatchDescription> GetAllQueuedBatchIds()
        {
            var description = GetQueue(_workQueueName);
            var allMessages = _workQueueClient.PeekBatch(0, (int)description.MessageCount);
            var uniqueBatchDescriptions = new Dictionary<Guid, BatchDescription>();
            foreach (var message in allMessages)
            {
                var job = message.GetBody<Job<InputT>>();

                if (!uniqueBatchDescriptions.ContainsKey(job.BatchId))
                    uniqueBatchDescriptions.Add(job.BatchId, new BatchDescription(job.BatchId));
                var batchDescription = uniqueBatchDescriptions[job.BatchId];
                batchDescription.NoOfJobs++;
            }
            return uniqueBatchDescriptions.Values;
        }
    }
}
