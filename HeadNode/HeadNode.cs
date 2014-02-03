using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NLog;

using SimpleScale.Common;

namespace SimpleScale.HeadNode
{
    public class HeadNode<T, U>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IQueueManager<T, U> _queueManager;
        private Task _thread { get; set; }

        public IDictionary<Guid, BatchProgress> BatchProgressDictionary = new Dictionary<Guid, BatchProgress>();

        public HeadNode(IQueueManager<T, U> queueManager)
        {
            _queueManager = queueManager;
        }

        public void RunBatch(Batch<T> batch)
        {
            _logger.Info("Adding batch '" + batch.Id + "' to queue...");
            var jobs = batch.JobDataList.Select((jobData, index) => new Job<T>(jobData, index, batch.Id)).ToList();
            var batchProgress = new BatchProgress{ ItemsInBatch = batch.JobDataList.Count };
            BatchProgressDictionary.Add(batch.Id, batchProgress);
            _queueManager.AddJobs(jobs);
        }

        public void StartHeadNode(CancellationTokenSource cancellationTokenSource)
        {
            CancellationToken token = cancellationTokenSource.Token;
            Task.Factory.StartNew(() => StartHeadNodeAsync(token), token);
        }

        public void StartHeadNodeAsync(CancellationToken token)
        {
            _logger.Info("Head node thread started.");
            while (true)
            {
                var completedJobResult = _queueManager.ReadCompletedJob();
                var batchProgress = BatchProgressDictionary[completedJobResult.BatchId];
                if (batchProgress.ListOfCompletedJobs.Contains(completedJobResult.Id) == false)
                    batchProgress.ListOfCompletedJobs.Add(completedJobResult.Id);
                if (token.IsCancellationRequested)
                {
                    _logger.Info("Head node thread cancelled.");
                    break;
                }
                Thread.Sleep(20);
            }
        }

    }
}
