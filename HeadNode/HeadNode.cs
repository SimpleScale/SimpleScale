using System;
using System.Collections.Generic;
using System.Linq;

using NLog;

using SimpleScale.Common;

namespace SimpleScale.HeadNode
{
    public class HeadNode<T>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IQueueManager<T> _queueManager;

        public HeadNode(IQueueManager<T> queueManager)
        {
            _queueManager = queueManager;
        }

        public void RunBatch(Batch<T> batch)
        {
            _logger.Info("Adding batch '" + batch.Id + "' to queue...");
            var jobs = batch.JobDataList.Select((jobData, index) => new Job<T>(jobData, index, batch.Id)).ToList();
            _queueManager.Add(jobs);
        }
    }
}
