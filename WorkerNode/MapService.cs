using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NLog;

using SimpleScale.Common;

namespace SimpleScale.WorkerNode
{
    public class MapService<T>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        readonly IQueueManager<T> _queueManager;
        readonly IMapJob<T> _mapJob;
        public MapService(IQueueManager<T> queueManager, IMapJob<T> mapJob)
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
            _logger.Info("Starting the map service " + Thread.CurrentThread.ManagedThreadId + ".");
            while (true)
            {
                var job = _queueManager.Read();
                _logger.Info("Processing job " + job.Id + ".");
                _mapJob.DoWork(job);
                _logger.Info("Job " + job.Id + " complete.");
                cancellationToken.ThrowIfCancellationRequested();
            }
            _logger.Info("Ending the map service " + Thread.CurrentThread.ManagedThreadId + ".");
        }
    }
}
