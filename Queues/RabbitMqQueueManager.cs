using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Queues
{
    public class RabbitMqQueueManager<InputT, ResultU> : IQueueManager<InputT, ResultU>
    {
        public void AddJobs(List<Common.Job<InputT>> jobs)
        {
            throw new NotImplementedException();
        }

        public bool ReadJobAndDoWork(Func<Common.Job<InputT>, ResultU> doWork, out Common.Job<InputT> job, out ResultU results)
        {
            throw new NotImplementedException();
        }

        public void AddCompleteJob(Common.Result<ResultU> job)
        {
            throw new NotImplementedException();
        }

        public bool ReadCompletedJob(out Common.Result<ResultU> result)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
