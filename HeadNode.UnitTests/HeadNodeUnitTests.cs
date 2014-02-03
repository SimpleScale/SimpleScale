using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NUnit.Framework;

namespace SimpleScale.HeadNode.UnitTests
{
    [TestFixture]
    public class HeadNodeUnitTests
    {
        Common.MemoryQueueManager<int, int> _queueManager;
        HeadNode.HeadNode<int, int> _headNode;
        List<int> _jobDataList;
        Common.Batch<int> _batch;
        CancellationTokenSource _cancellationTokenSource;

        [SetUp]
        public void Setup()
        {
            _queueManager = new Common.MemoryQueueManager<int, int>();
            _jobDataList = new List<int> { 1, 2, 3 };
            _batch = new Common.Batch<int>(_jobDataList);
            _cancellationTokenSource = new CancellationTokenSource();
            _headNode = new HeadNode.HeadNode<int, int>(_queueManager);
        }

        [Test]
        public void TestCTor()
        {
            var headNode = new HeadNode.HeadNode<int, int>(_queueManager);
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
            _queueManager.AddCompleteJob(new Common.Result<int>(1, 1, _batch.Id));
            Thread.Sleep(50);

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
            _queueManager.AddCompleteJob(resultJob1);
            _queueManager.AddCompleteJob(resultJob1);
            Thread.Sleep(50);

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
            _queueManager.AddCompleteJob(new Common.Result<int>(1, 1, _batch.Id));
            _queueManager.AddCompleteJob(new Common.Result<int>(1, 2, _batch.Id));
            Thread.Sleep(50);

            var batchProgress = _headNode.BatchProgressDictionary[_batch.Id];
            Assert.AreEqual(2, batchProgress.ListOfCompletedJobs.Count);
            Assert.AreEqual(1, batchProgress.ListOfCompletedJobs[0]);
            Assert.AreEqual(2, batchProgress.ListOfCompletedJobs[1]);
            _cancellationTokenSource.Cancel();
        }
    }
}
