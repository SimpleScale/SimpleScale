using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NUnit.Framework;

using SimpleScale.Queues;

namespace SimpleScale.HeadNode.UnitTests
{
    [TestFixture]
    public class HeadNodeUnitTests
    {
        MemoryQueueManager<int, int> _memoryQueueManager;
        HeadNode.HeadNode<int, int> _headNode;
        List<int> _jobDataList;
        Common.Batch<int> _batch;
        CancellationTokenSource _cancellationTokenSource;
        Guid _batchId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _memoryQueueManager = new MemoryQueueManager<int, int>();
            _memoryQueueManager.SleepInterval = 0;
            _jobDataList = new List<int> { 1, 2, 3 };
            _batch = new Common.Batch<int>(_jobDataList);
            _cancellationTokenSource = new CancellationTokenSource();
            _headNode = new HeadNode.HeadNode<int, int>(_memoryQueueManager);
        }

        [Test]
        public void TestCTor()
        {
            var headNode = new HeadNode.HeadNode<int, int>(_memoryQueueManager);
            Assert.AreEqual(0, headNode.BatchProgressDictionary.Count);
        }

        [Test]
        public void RunBatchWithOneItemBatchCountIsOne()
        {
            _headNode.RunBatch(_batch);
            Assert.AreEqual(1, _headNode.BatchProgressDictionary.Count);
        }

        [Test]
        public void RunBatchWithThreeItemsProgessIsZero()
        {
            _headNode.RunBatch(_batch);
            var batchProgress = _headNode.BatchProgressDictionary[_batch.Id];
            Assert.AreEqual(3, batchProgress.ItemsInBatch);
            Assert.AreEqual(0, batchProgress.ListOfCompletedJobs.Count);
        }

        [Test]
        public void RunBatchWithThreeItemsProgressJobProgessIsOne()
        {
            _headNode.RunBatch(_batch);
            _headNode.StartHeadNode(_cancellationTokenSource);
            _memoryQueueManager.AddCompleteJob(new Common.Result<int>(1, 1, _batch.Id));
            Thread.Sleep(10);

            var batchProgress = _headNode.BatchProgressDictionary[_batch.Id];
            Assert.AreEqual(1, batchProgress.ListOfCompletedJobs.Count);
            Assert.AreEqual(1, batchProgress.ListOfCompletedJobs[0]);
            _cancellationTokenSource.Cancel();
        }

        [Test]
        public void RunBatchWithThreeItemsProgressSameJobTwiceJobProgessIsOne()
        {
            _headNode.RunBatch(_batch);
            _headNode.StartHeadNode(_cancellationTokenSource);
            var resultJob1 = new Common.Result<int>(1, 1, _batch.Id);
            _memoryQueueManager.AddCompleteJob(resultJob1);
            _memoryQueueManager.AddCompleteJob(resultJob1);
            Thread.Sleep(10);

            var batchProgress = _headNode.BatchProgressDictionary[_batch.Id];
            Assert.AreEqual(1, batchProgress.ListOfCompletedJobs.Count);
            Assert.AreEqual(1, batchProgress.ListOfCompletedJobs[0]);
            _cancellationTokenSource.Cancel();
        }

        [Test]
        public void RunBatchWithThreeItemsProgressTwoJobsJobProgessIsTwo()
        {
            _headNode.RunBatch(_batch);
            _headNode.StartHeadNode(_cancellationTokenSource);
            _memoryQueueManager.AddCompleteJob(new Common.Result<int>(1, 1, _batch.Id));
            _memoryQueueManager.AddCompleteJob(new Common.Result<int>(1, 2, _batch.Id));
            Thread.Sleep(30);

            var batchProgress = _headNode.BatchProgressDictionary[_batch.Id];
            Assert.AreEqual(2, batchProgress.ListOfCompletedJobs.Count);
            Assert.AreEqual(1, batchProgress.ListOfCompletedJobs[0]);
            Assert.AreEqual(2, batchProgress.ListOfCompletedJobs[1]);
            _cancellationTokenSource.Cancel();
        }

        [Test]
        public void IfBatchNotCompleteDontRaiseEvent(){
            _headNode.BatchProgressDictionary.Add(_batchId, new BatchProgress { ItemsInBatch = 1 });
            _headNode.BatchComplete += batchCompleteFail;
            _headNode.RaiseBatchCompleteEventIfBatchComplete(_batchId);
            Assert.Pass();
        }

        void batchCompleteFail(object sender, BatchCompleteEventArgs e)
        {
            Assert.Fail();
        }

        [Test]
        public void IfBatchCompleteRaiseEvent()
        {
            var batchProgress = new BatchProgress { ItemsInBatch = 1 };
            batchProgress.ListOfCompletedJobs.Add(1);
            _headNode.BatchProgressDictionary.Add(_batchId, batchProgress);
            _headNode.BatchComplete += batchCompletePass;
            _headNode.RaiseBatchCompleteEventIfBatchComplete(_batchId);
            Assert.Fail();
        }

        void batchCompletePass(object sender, BatchCompleteEventArgs e)
        {
            Assert.Pass();
        }
    }
}
