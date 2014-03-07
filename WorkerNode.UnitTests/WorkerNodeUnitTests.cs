using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NUnit.Framework;

using SimpleScale.Common;
using SimpleScale.Queues;

namespace SimpleScale.WorkerNode.UnitTests
{

    public class FailingQueueManager<T, U> : MemoryQueueManager<T, U>
    {
        public override bool ReadJobAndDoWork(Func<Job<T>, U> doWork, out Job<T> job, out U results)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class WorkerNodeUnitTests
    {
        MemoryQueueManager<int, int> _memoryQueueManager;
        FailingQueueManager<int, int> _failingQueueManager;
        WorkerNode.WorkerNode<int, int> _workerNode;
        static Guid BatchId = Guid.NewGuid();
        const int JobId = 2;
        Job<int> job = new Job<int>(1, JobId, BatchId);

        [SetUp]
        public void Setup()
        {
            _memoryQueueManager = new MemoryQueueManager<int, int>();
            _memoryQueueManager.SleepInterval = 0;

            _failingQueueManager = new FailingQueueManager<int, int>();
            _failingQueueManager.SleepInterval = 0;
        }
        
        [Test]
        public void WhenJobThrowsErrorResultHasError()
        {
            _workerNode = new WorkerNode<int, int>(_memoryQueueManager, new FailingJobRunner());
            _memoryQueueManager.AddJobs(new List<Job<int>> { job });
            _workerNode.StartAsync();
            Result<int> result;
            Thread.Sleep(10);
            if (_memoryQueueManager.ReadCompletedJob(out result))
            {
                Assert.That(result.HasError, Is.True);
                Assert.That(result.Exception, Is.TypeOf<System.NotImplementedException>());
                Assert.That(result.BatchId, Is.EqualTo(BatchId));
                Assert.That(result.Id, Is.EqualTo(JobId));
            }
            else
                Assert.Fail();
        }

        [Test]
        public void WhenQueueManagerThrowsErrorResultHasError()
        {
            _workerNode = new WorkerNode<int, int>(_failingQueueManager, new TimesTwoJobRunner());
            _failingQueueManager.AddJobs(new List<Job<int>> { job });
            _workerNode.StartAsync();
            Result<int> result;
            Thread.Sleep(10);
            if (_failingQueueManager.ReadCompletedJob(out result))
            {
                Assert.That(result.HasError, Is.True);
                Assert.That(result.Exception, Is.TypeOf<System.NotImplementedException>());
                Assert.That(result.BatchId, Is.EqualTo(Guid.Empty));
                Assert.That(result.Id, Is.EqualTo(Job<int>.UnknownJobId));
            }
            else
                Assert.Fail();
        }
    }
}
