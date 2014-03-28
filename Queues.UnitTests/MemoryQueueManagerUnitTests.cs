using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using SimpleScale.Common;
using SimpleScale.Queues;

namespace Queues.UnitTests
{
    public class MemoryQueueManagerUnitTests
    {
        MemoryQueueManager<int, string> _memoryQueueManager;
        
        Guid _batchId1;
        Guid _batchId2;
        Guid _batchId3;

        Job<int> _job1Batch1;
        Job<int> _job1Batch2;
        Job<int> _job1Batch3;
        Job<int> _job2Batch3;

        [SetUp]
        public void Setup()
        {
            _memoryQueueManager = new MemoryQueueManager<int, string>();
            _batchId1 = Guid.NewGuid();
            _batchId2 = Guid.NewGuid();
            _batchId3 = Guid.NewGuid();
            _job1Batch1 = new Job<int>(1, 2, _batchId1);
            _job1Batch2 = new Job<int>(1, 2, _batchId2);
            _job1Batch3 = new Job<int>(1, 2, _batchId3);
            _job2Batch3 = new Job<int>(1, 2, _batchId3);
        }

        [Test]
        public void GetAllQueuedBatchIdsWhenQueueIsEmptyNothingReturned()
        {
            var batchesInQueue = _memoryQueueManager.GetAllQueuedBatchIds();
            Assert.That(batchesInQueue.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetAllQueuedBatchIdsWhenQueueHasOneBatchOneBatchReturned()
        {
            _memoryQueueManager.AddJobs(new List<Job<int>> { _job1Batch1 });
            var batchesInQueue = _memoryQueueManager.GetAllQueuedBatchIds().ToList();
            Assert.That(batchesInQueue.Count(), Is.EqualTo(1));
            Assert.That(batchesInQueue[0].Id, Is.EqualTo(_batchId1));
            Assert.That(batchesInQueue[0].NoOfJobs, Is.EqualTo(1));
            
        }

        [Test]
        public void GetAllQueuedBatchIdsWhenQueueHasThreeBatchesThreeBatchReturned()
        {
            _memoryQueueManager.AddJobs(new List<Job<int>> { _job1Batch1, _job1Batch2, _job1Batch3, _job2Batch3 });
            var batchesInQueue = _memoryQueueManager.GetAllQueuedBatchIds().ToList();
            Assert.That(batchesInQueue.Count(), Is.EqualTo(3));
            Assert.That(batchesInQueue[0].Id, Is.EqualTo(_batchId1));
            Assert.That(batchesInQueue[1].Id, Is.EqualTo(_batchId2));
            Assert.That(batchesInQueue[2].Id, Is.EqualTo(_batchId3));
            Assert.That(batchesInQueue[2].NoOfJobs, Is.EqualTo(2));
        }

    }
}
