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
    public class ServiceBusQueueManagerUnitTests
    {
        ServiceBusQueueManager<int, string> _serviceBusQueueManager;
        
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
        }

    }
}
