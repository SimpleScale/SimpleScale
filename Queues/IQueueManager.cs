using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace SimpleScale.Queues
{
    public interface IQueueManager<InputT, ResultU> : IDisposable
    {
        void AddJobs(List<Job<InputT>> jobs);
        bool ReadJobAndDoWork(Func<Job<InputT>, ResultU> doWork, out Job<InputT> job, out ResultU result);
        void AddCompleteJob(Result<ResultU> result);
        bool ReadCompletedJob(out Result<ResultU> result);
        IEnumerable<BatchDescription> GetAllQueuedBatchIds();
    }
}
