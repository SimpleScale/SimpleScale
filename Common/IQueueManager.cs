using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IQueueManager<T, U>
    {
        void AddJobs(List<Job<T>> jobs);
        Job<T> ReadJob();
        void AddCompleteJob(Result<U> job);
        Result<U> ReadCompletedJob();
    }
}
