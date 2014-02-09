using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IQueueManager1<T, U>
    {
        void AddJobs(List<Job<T>> jobs);
        bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U results);
        void AddCompleteJob(Result<U> job);
        Result<U> ReadCompletedJob();
    }
}