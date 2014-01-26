using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NLog;

namespace SimpleScale.HeadNode
{
    public class Job<T>
    {
        private int _Id;
        private T _info;
        public Job(int id, T info)
        {
            _Id = id;
            _info = info;
        }

        public int Id { get { return _Id; } }
        public T Info { get { return _info; } }

    }

    public interface IQueueManager<T>
    {
        void Add(List<Job<T>> jobs);
        Job<T> Read();
    }

    public class MemoryQueueManager<T> : IQueueManager<T>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<Job<T>> _jobs = new ConcurrentQueue<Job<T>>();
        public MemoryQueueManager() {}

        public void Add(List<Job<T>> jobs)
        {
            _logger.Info("Adding " + jobs.Count + " jobs.");
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
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        readonly IQueueManager<T> _queueManager;
        readonly IMapJob<T> _mapJob;
        public MapService(IQueueManager<T> queueManager, IMapJob<T> mapJob)
        {
            _queueManager = queueManager;
            _mapJob = mapJob;
        }

        public void StartAsync(CancellationTokenSource cancellationTokenSource)
        {
            var token = cancellationTokenSource.Token;
            Task.Factory.StartNew(() => Start(token), token);
        }

        public void Start(CancellationToken cancellationToken)
        {
            _logger.Info("Starting the map service " + Thread.CurrentThread.ManagedThreadId + ".");
            while (true){
                var job = _queueManager.Read();
                _logger.Info("Processing job " + job.Id + ".");
                _mapJob.DoWork(job);
                _logger.Info("Job " + job.Id + " complete.");
                cancellationToken.ThrowIfCancellationRequested();
            }
            _logger.Info("Ending the map service " + Thread.CurrentThread.ManagedThreadId + ".");
        }

    }
}
