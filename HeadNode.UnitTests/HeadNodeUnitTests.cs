using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace SimpleScale.HeadNode.UnitTests
{
    [TestFixture]
    public class HeadNodeUnitTests
    {
        [Test]
        public void TestCTor()
        {
            var queueManager = new Common.MemoryQueueManager<int>();
            var headNode = new HeadNode.HeadNode<int>(queueManager);
        }
    }
}
