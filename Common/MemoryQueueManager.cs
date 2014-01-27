using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NLog;

namespace SimpleScale.Common
{
    public class MemoryQueueManager<T> : IQueueManager<T>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<Job<T>> _jobs = new ConcurrentQueue<Job<T>>();
        public MemoryQueueManager() { }

        public void Add(List<Job<T>> jobs)
        {
            jobs.ForEach(j => _jobs.Enqueue(j));
            _logger.Info(_jobs.Count + " jobs in the queue.");
        }

        public Job<T> Read()
        {
            Job<T> returnValue;
            while (true)
            {
                if (_jobs.TryDequeue(out returnValue))
                    return returnValue;
                Thread.Sleep(100);
            }
        }
    }
}
