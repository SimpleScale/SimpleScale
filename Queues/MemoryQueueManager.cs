using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NLog;

using SimpleScale.Common;

namespace SimpleScale.Queues
{
    public class MemoryQueueManager<T, U> : IQueueManager<T, U>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<Job<T>> _jobs = new ConcurrentQueue<Job<T>>();
        private ConcurrentQueue<Result<U>> _completedJobs = new ConcurrentQueue<Result<U>>();
        
        public MemoryQueueManager() { }
        public int SleepInterval = 100;
        public void AddJobs(List<Job<T>> jobs)
        {
            jobs.ForEach(j => _jobs.Enqueue(j));
            _logger.Info(_jobs.Count + " jobs in the queue.");
        }

        public bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U result)
        {
            if (_jobs.TryDequeue(out job))
            {
                result = doWork(job);
                return true;
            }
            result = default(U);
            Thread.Sleep(SleepInterval);
            return false;
        }

        public void AddCompleteJob(Result<U> result)
        {
            _completedJobs.Enqueue(result);
            _logger.Info(result.Id + " completed.");
        }

        public Result<U> ReadCompletedJob()
        {
            Result<U> returnValue;
            while (true)
            {
                if (_completedJobs.TryDequeue(out returnValue))
                    return returnValue;
                Thread.Sleep(SleepInterval);
            }
        }
    }
}
