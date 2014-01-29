using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NLog;

using SimpleScale.Common;

namespace SimpleScale.WorkerNode
{
    public class WorkerNode<T>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IQueueManager<T> _queueManager;
        private readonly IMapJob<T> _mapJob;

        public WorkerNode(IQueueManager<T> queueManager, IMapJob<T> mapJob)
        {
            _queueManager = queueManager;
            _mapJob = mapJob;
        }

        public void StartAsync(CancellationTokenSource cancellationTokenSource)
        {
            var token = cancellationTokenSource.Token;
            Task.Factory.StartNew(() => Start(token), token);
        }

        public void Start(CancellationToken cancellationToken)
        {
            var threadIdText = "(Thread Id -" + Thread.CurrentThread.ManagedThreadId + ").";
            _logger.Info(threadIdText + " Starting a worker node...");
            while (true)
            {
                var job = _queueManager.Read();
                _logger.Info(threadIdText + " Processing job '" + job.Id + "' in batch '" + job.BatchId + "'.");
                _mapJob.DoWork(job);
                _logger.Info(threadIdText + "Job '" + job.Id + "' in batch '" + job.BatchId + "' completed.");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
