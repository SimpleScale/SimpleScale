using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NLog;

using SimpleScale.Common;
using SimpleScale.Queues;

namespace SimpleScale.HeadNode
{
    public class HeadNode<T, U>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IQueueManager<T, U> _queueManager;
        private Task _thread { get; set; }

        public delegate void JobCompleteEventHandler(object sender, JobCompleteEventArgs<U> e);
        public event JobCompleteEventHandler JobComplete;

        public HeadNode(IQueueManager<T, U> queueManager)
        {
            _queueManager = queueManager;
        }
        
        public void RunBatch(Batch<T> batch)
        {
            _logger.Info("Adding batch '" + batch.Id + "' to queue...");
            _queueManager.AddJobs(batch.Jobs);
            _logger.Info(batch.Jobs.Count + " jobs added to queue.");
        }

        public void StartHeadNode()
        {
            Task.Factory.StartNew(StartHeadNodeAsync);
        }

        private void StartHeadNodeAsync()
        {
            _logger.Info("Head node thread started.");
            while (true)
            {
                
                Result<U> completedJobResult;
                if (!_queueManager.ReadCompletedJob(out completedJobResult))
                    continue;
                RaiseJobCompleteEvent(completedJobResult);
            }
        }

        private void RaiseJobCompleteEvent(Result<U> result)
        {
            if (JobComplete != null)
            {
                var jobCompleteEventArgs = new JobCompleteEventArgs<U>(result);
                JobComplete(this, jobCompleteEventArgs);
            }
        }
    }
}
