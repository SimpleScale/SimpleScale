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
    public class MemoryQueueManager<InputT, ResultU> : IQueueManager<InputT, ResultU>
    {
        private ConcurrentQueue<Job<InputT>> _jobs = new ConcurrentQueue<Job<InputT>>();
        private ConcurrentQueue<Result<ResultU>> _completedJobs = new ConcurrentQueue<Result<ResultU>>();
        
        public MemoryQueueManager() { }
        public int SleepInterval = 100;
        public void AddJobs(List<Job<InputT>> jobs)
        {
            jobs.ForEach(j => _jobs.Enqueue(j));
        }

        public virtual bool ReadJobAndDoWork(Func<Job<InputT>, ResultU> doWork, out Job<InputT> job, out ResultU result)
        {
            if (_jobs.TryDequeue(out job))
            {
                result = doWork(job);
                return true;
            }
            result = default(ResultU);
            Thread.Sleep(SleepInterval);
            return false;
        }

        public void AddCompleteJob(Result<ResultU> result)
        {
            _completedJobs.Enqueue(result);
        }

        public bool ReadCompletedJob(out Result<ResultU> result)
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
