using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.Serialization;

namespace SimpleScale.Queues
{
    public class RabbitMqQueueManager<InputT, ResultU> : IQueueManager<InputT, ResultU>
    {
        private readonly IConnection _connection;
        private const ushort PrefetchCount = 1;
        private readonly string _workQueueName;
        private readonly string _workCompletedQueueName;

        public RabbitMqQueueManager(string hostName, string workQueueName, string workCompletedQueueName){
            _workQueueName = workQueueName;
            _workCompletedQueueName = workCompletedQueueName;
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
        }

        internal RabbitMqQueueManager() { }

        public void AddJobs(List<Common.Job<InputT>> jobs)
        {
            using (var model = _connection.CreateModel())
            {
                CreateQueue(_workQueueName, model);
                foreach (var job in jobs)
                {
                    var body = SerialiseMessage(job);
                    model.BasicPublish("", _workQueueName, null, body);
                }
            }
        }

        public bool ReadJobAndDoWork(Func<Common.Job<InputT>, ResultU> doWork, out Common.Job<InputT> job, out ResultU result)
        {
            job = null;
            result = default(ResultU);
            using (var model = _connection.CreateModel())
            {
                CreateQueue(_workQueueName, model);
                var consumer = new QueueingBasicConsumer(model);
                model.BasicConsume(_workQueueName, false, consumer);

                BasicDeliverEventArgs basicDeliverEventArgs;
                consumer.Queue.Dequeue(500, out basicDeliverEventArgs);
                if (basicDeliverEventArgs == null)
                    return false;

                job = DeserialiseMessage<Common.Job<InputT>>(basicDeliverEventArgs.Body);
                result = doWork(job);
                model.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
            return true;
        }

        public void AddCompleteJob(Common.Result<ResultU> result)
        {
            using (var model = _connection.CreateModel())
            {
                CreateQueue(_workCompletedQueueName, model);
                var body = SerialiseMessage(result);
                model.BasicPublish("", _workCompletedQueueName, null, body);
            }
        }

        public bool ReadCompletedJob(out Common.Result<ResultU> result)
        {
            result = null;
            using (var model = _connection.CreateModel())
            {
                CreateQueue(_workCompletedQueueName, model);
                var consumer = new QueueingBasicConsumer(model);
                model.BasicConsume(_workCompletedQueueName, false, consumer);

                BasicDeliverEventArgs basicDeliverEventArgs;
                consumer.Queue.Dequeue(500, out basicDeliverEventArgs);
                if (basicDeliverEventArgs == null)
                    return false;

                result = DeserialiseMessage<Common.Result<ResultU>>(basicDeliverEventArgs.Body);                
                model.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
            return true;
        }

        public void Dispose()
        {
            _connection.Close();
        }

        private void CreateQueue(string queueName, IModel model)
        {
            model.QueueDeclare(queueName, true, false, false, null);
            model.BasicQos(0, PrefetchCount, false);
        }

        internal byte[] SerialiseMessage<T>(T job)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, job);
            return memoryStream.ToArray();
        }

        internal T DeserialiseMessage<T>(byte[] body)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            memoryStream.Write(body, 0, body.Length);
            memoryStream.Position = 0;
            return (T)serializer.ReadObject(memoryStream);
        }
    }
}
