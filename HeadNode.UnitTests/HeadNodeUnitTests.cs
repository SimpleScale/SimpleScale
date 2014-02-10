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
        }


    }
}
