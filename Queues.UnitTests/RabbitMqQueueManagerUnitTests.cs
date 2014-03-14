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
    public class RabbitMqQueueManagerUnitTests
    {
        RabbitMqQueueManager<int, string> _rabbitMqQueueManager;
        Job<int> _job;
        Result<string> _result;

        [SetUp]
        public void Setup()
        {
            _rabbitMqQueueManager = new RabbitMqQueueManager<int, string>();
            _job = new Job<int>(1, 2, Guid.NewGuid());
            _result = new Result<string>("test", 3, Guid.NewGuid());
        }

        [Test]
        public void SerialiaseJob()
        {
            var serialisedJob = _rabbitMqQueueManager.SerialiseMessage(_job);
            Assert.That(serialisedJob.Length, Is.GreaterThan(0));
        }

        [Test]
        public void DeserialiaseJob()
        {
            var serialisedJob = _rabbitMqQueueManager.SerialiseMessage(_job);
            var job = _rabbitMqQueueManager.DeserialiseMessage<Job<int>>(serialisedJob);
            Assert.That(job.Id, Is.EqualTo(_job.Id));
            Assert.That(job.BatchId, Is.EqualTo(_job.BatchId));
            Assert.That(job.Data, Is.EqualTo(_job.Data));
        }

        [Test]
        public void SerialiaseResult()
        {
            var serialisedResult = _rabbitMqQueueManager.SerialiseMessage(_result);
            Assert.That(serialisedResult.Length, Is.GreaterThan(0));
        }

        [Test]
        public void DeserialiaseResult()
        {
            var serialisedResult = _rabbitMqQueueManager.SerialiseMessage(_result);
            var result = _rabbitMqQueueManager.DeserialiseMessage<Result<string>>(serialisedResult);
            Assert.That(result.Id, Is.EqualTo(_result.Id));
            Assert.That(result.BatchId, Is.EqualTo(_result.BatchId));
            Assert.That(result.Data, Is.EqualTo(_result.Data));
        }
    }
}
