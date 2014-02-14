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
    public class ServiceBusQueueManager<T, U> : IQueueManager<T, U> 
    {
        private readonly NamespaceManager _namespaceManager;
        private readonly MessagingFactory _messagingFactory;
        private readonly QueueClient _workQueueClient;
        private readonly QueueClient _workCompletedQueueClient;

        public ServiceBusQueueManager(string connectionString, string workQueueName, string workCompletedQueueName)
        {
            _namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            _messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);

            _workQueueClient = CreateQueueClient(workQueueName);
            _workCompletedQueueClient = CreateQueueClient(workCompletedQueueName);
        }

        public void AddJobs(List<Job<T>> jobs)
        {
            var messagesList = jobs.Select(job => new BrokeredMessage(job));
            _workQueueClient.SendBatch(messagesList);
        }

        public bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U result)
        {
            job = null;
            result = default(U);
            var message = _workQueueClient.Receive(new TimeSpan(0, 0, 0, 2));
            if (message == null)
                return false;
            job = message.GetBody<Job<T>>();
            result = doWork(job);
            _workQueueClient.Complete(message.LockToken);
            return true;
        }

        public void AddCompleteJob(Result<U> result)
        {
            _workCompletedQueueClient.Send(new BrokeredMessage(result));
        }

        public bool ReadCompletedJob(out Result<U> result)
        {
            result = null;
            var message = _workCompletedQueueClient.Receive(new TimeSpan(0, 0, 0, 2));
            if (message == null)
                return false;
            result = message.GetBody<Result<U>>();
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
    }
}
