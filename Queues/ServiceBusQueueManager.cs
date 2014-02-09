using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NLog;

using SimpleScale.Common;

namespace SimpleScale.Queues
{
    public class ServiceBusQueueManager<T, U> : IQueueManager<T, U>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();

        public ServiceBusQueueManager() { }
        public void AddJobs(List<Job<T>> jobs)
        {
            //_logger.Info(_jobs.Count + " jobs in the queue.");
        }

        public bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U result)
        {
            job = null;
            if (true)
            {
                result = doWork(job);
                return true;
            }
            return false;
        }

        public void AddCompleteJob(Result<U> result)
        {
            //_completedJobs.Enqueue(result);
            _logger.Info(result.Id + " completed.");
        }

        public Result<U> ReadCompletedJob()
        {
            Result<U> returnValue = null;
            return returnValue;
        }
    }
}
