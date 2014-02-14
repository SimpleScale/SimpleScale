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
        private ConcurrentQueue<Job<T>> _jobs = new ConcurrentQueue<Job<T>>();
        private ConcurrentQueue<Result<U>> _completedJobs = new ConcurrentQueue<Result<U>>();
        
        public MemoryQueueManager() { }
        public int SleepInterval = 100;
        public void AddJobs(List<Job<T>> jobs)
        {
            jobs.ForEach(j => _jobs.Enqueue(j));
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
        }

        public bool ReadCompletedJob(out Result<U> result)
        {
            Thread.Sleep(SleepInterval);
            if (_completedJobs.TryDequeue(out result))
                return true;
            return false;
        }

        public void Dispose()
        {            
        }
    }
}
