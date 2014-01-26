using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimpleScale.HeadNode
{
    public class Job<T>
    {
        private int _Id;
        private T _jobInfo;
        public Job(int id, T jobInfo)
        {
            _Id = id;
            _jobInfo = jobInfo;
        }
    }

    public interface IQueueManager<T>
    {
        void Add(List<Job<T>> jobs);
        Job<T> Read();
    }

    public class MemoryQueueManager<T> : IQueueManager<T>
    {
        private ConcurrentQueue<Job<T>> _jobs = new ConcurrentQueue<Job<T>>();
        public MemoryQueueManager() {}

        public void Add(List<Job<T>> jobs)
        {
            jobs.ForEach(j => _jobs.Enqueue(j));
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

    public interface IMapJob<T> {
        void DoWork(Job<T> job);
    }

    public class MapService<T>
    {
        readonly IQueueManager<T> _queueManager;
        readonly IMapJob<T> _mapJob;
        public MapService(IQueueManager<T> queueManager, IMapJob<T> mapJob)
        {
            _queueManager = queueManager;
            _mapJob = mapJob;
        }

        public void Start()
        {
            while (true){
                var job = _queueManager.Read();
                _mapJob.DoWork(job);
            }
        }

    }
}
