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
        public static Logger _logger = LogManager.GetCurrentClassLogger();

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
            _logger.Info(jobs.Count + " jobs added to queue.");
        }

        public bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U result)
        {
            job = null;
            result = default(U);
            var message = _workQueueClient.Receive(new TimeSpan(0, 0, 0, 2));
            if (message == null)
                return false;
            job = message.GetBody<Job<T>>();
            _logger.Info(job.Id + " started.");
            result = doWork(job);
            _logger.Info(job.Id + " completed.");
            _workQueueClient.Complete(message.LockToken);
            return true;
        }

        public void AddCompleteJob(Result<U> result)
        {
            _workCompletedQueueClient.Send(new BrokeredMessage(result));
            _logger.Info(result.Id + " completed.");
        }

        public Result<U> ReadCompletedJob()
        {
            Result<U> returnValue = null;
            return returnValue;
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
