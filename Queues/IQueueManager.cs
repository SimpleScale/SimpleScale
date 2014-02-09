using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace SimpleScale.Queues
{
    public interface IQueueManager<T, U> : IDisposable
    {
        void AddJobs(List<Job<T>> jobs);
        bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U results);
        void AddCompleteJob(Result<U> job);
        Result<U> ReadCompletedJob();
    }
}
