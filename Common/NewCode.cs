//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace QueueingAPI.Queues
//{
//    public interface IQueueMonitor
//    {
//        long GetNoMessageInQueue();
//    }
//}

//namespace QueueingAPI.Queues.ServiceBus
//{
//    public class ServiceBusMonitor : IQueueMonitor
//    {
//        private readonly ServiceBusQueue _serviceBusQueue;

//        public ServiceBusMonitor(ServiceBusQueue serviceBusQueue)
//        {
//            _serviceBusQueue = serviceBusQueue;
//        }

//        public long GetNoMessageInQueue()
//        {
//            var queue = _serviceBusQueue.GetQueue();
//            return queue.MessageCount;
//        }
//    }
//}


//namespace QueueingAPI.Queues
//{
//    public enum MessageType
//    {
//        ReceiverStarted,
//        ReceiverEnded,
//        BatchSent,
//        WorkCompleted,
//        GenericMessage,
//        WorkFailed
//    }

//    public class MessageEventArgs : EventArgs
//    {
//        public MessageType MessageType;
//        public string Message;
//        public int ThreadId;
//    }

//    public abstract class JobManager<T>
//    {
//        public event MessageHandler MessageEvent;
//        public delegate void MessageHandler(MessageEventArgs e);

//        public abstract void SendJobs(IEnumerable<T> jobMessageList);
//        public abstract void ReceiveMessage(Action<T> doWork, CancellationTokenSource cancellationTokenSource);

//        protected void RaiseMessageEvent(MessageType messageType)
//        {
//            RaiseMessageEvent(messageType, String.Empty);
//        }

//        protected void RaiseMessageEvent(MessageType messageType, string message)
//        {
//            if (MessageEvent != null)
//            {
//                var threadId = Thread.CurrentThread.ManagedThreadId;
//                MessageEvent(new MessageEventArgs { Message = message, MessageType = messageType, ThreadId = threadId });
//            }
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Transactions;
//using Microsoft.ServiceBus.Messaging;

//namespace QueueingAPI.Queues.ServiceBus
//{
//    public class ServiceBusJobManager<T> : JobManager<T>
//    {
//        private readonly ServiceBusQueue _serviceBusQueue;

//       public ServiceBusJobManager(ServiceBusQueue serviceBusQueue)
//        {
//            _serviceBusQueue = serviceBusQueue;
//        }

//        public override void SendJobs(IEnumerable<T> jobMessageList)
//        {
//            var messagesList = jobMessageList.Select(CreateBrokeredMessage);
//            _serviceBusQueue.GetQueueClient().SendBatch(messagesList);
//            RaiseMessageEvent(MessageType.BatchSent);
//        }

//        public override void ReceiveMessage(Action<T> doWork, CancellationTokenSource cancellationTokenSource)
//        {
//            RaiseMessageEvent(MessageType.ReceiverStarted);
//            var queue = _serviceBusQueue.GetQueueClient();
//            while (true)
//            {
//                try
//                {
//                    if (cancellationTokenSource.Token.IsCancellationRequested)
//                        break;

//                    var message = queue.Receive(new TimeSpan(0, 0, 0, 2));
//                    if (message == null)
//                        continue;

//                    if (cancellationTokenSource.Token.IsCancellationRequested)
//                    {
//                        queue.Abandon(message.LockToken);
//                        break;
//                    }

//                    var messageData = message.GetBody<T>();
//                    doWork(messageData);
//                    queue.Complete(message.LockToken);
//                    RaiseMessageEvent(MessageType.WorkCompleted);
//                }
//                catch(Exception ex)
//                {
//                    RaiseMessageEvent(MessageType.WorkFailed, ex.Message);
//                }
//            }
//            RaiseMessageEvent(MessageType.ReceiverEnded);
//        }

//        private BrokeredMessage CreateBrokeredMessage(T messageData)
//        {
//            return new BrokeredMessage(messageData);
//        }
//    }
//}



//using System;
//using System.Configuration;
//using Microsoft.ServiceBus;
//using Microsoft.ServiceBus.Messaging;

//namespace QueueingAPI.Queues.ServiceBus
//{
//    public class ServiceBusQueue : IDisposable
//    {
//        private readonly string _queueName;
//        private readonly string _connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
//        private readonly NamespaceManager _namespaceManager;
//        private readonly MessagingFactory _messagingFactory;

//        public ServiceBusQueue(string queueNameName)
//        {
//            _queueName = queueNameName;
//            _namespaceManager = NamespaceManager.CreateFromConnectionString(_connectionString);
//            _messagingFactory = MessagingFactory.CreateFromConnectionString(_connectionString);
//            CreateQueue();
//        }

//        public QueueDescription GetQueue()
//        {
//            return _namespaceManager.GetQueue(_queueName);
//        }

//        public QueueClient GetQueueClient()
//        {
//            return _messagingFactory.CreateQueueClient(_queueName, ReceiveMode.PeekLock);
//        }

//        public void CreateQueue()
//        {
//            if (!_namespaceManager.QueueExists(_queueName))
//                _namespaceManager.CreateQueue(_queueName);
//        }

//        public void Dispose()
//        {
//            _messagingFactory.Close();
//        }
//    }
//}

//using System;
//using System.Globalization;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using QueueingAPI.Models;
//using QueueingAPI.Queues;
//using QueueingAPI.Queues.EasyNetQ;
//using QueueingAPI.Queues.RabbitMq;
//using QueueingAPI.Queues.ServiceBus;


//namespace QueueingAPI
//{
//    public enum QueueType
//    {
//        ServiceBus,
//        RabbitMq,
//        EasyNetQ
//    }

//    public partial class Form1 : Form
//    {

//        // Use GLAELMAPP01
//        private const string ServiceBusQueueName = "ElmQueue";
//        private const string RabbitMqQueueName = "ElmQueue";
//        private const string EastNetQQueueName = "QueueingAPI_ElmJobInput:QueueingAPI_ELM";
//        private const QueueType DefaultQueue = QueueType.ServiceBus;
//        private SynchronizationContext _synchronizationContext;

//        private CancellationTokenSource _cancellationTokenSource;

//        private JobManager<ElmJobInput> _jobManager;
//        private IQueueMonitor _queueMonitor;

//        public Form1()
//        {
//            try
//            {
//                InitializeComponent();

//                SetupQueue(DefaultQueue);
//                SetupUi();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//                Close();
//            }
//        }

//        private void SetupQueue(QueueType queueType)
//        {
//            CancelThreads();
//            _jobManager = null;
//            _queueMonitor = null;

//            switch (queueType)
//            {
//                case QueueType.ServiceBus:
//                    var serviceBusQueue = new ServiceBusQueue(ServiceBusQueueName);
//                    _jobManager = new ServiceBusJobManager<ElmJobInput>(serviceBusQueue);
//                    _queueMonitor = new ServiceBusMonitor(serviceBusQueue);
//                    break;
//                case QueueType.RabbitMq:
//                    var rabbitMqQueue = new RabbitMqQueue(RabbitMqQueueName);
//                    _jobManager = new RabbitMqJobManager<ElmJobInput>(rabbitMqQueue);
//                    _queueMonitor = new RabbitMqMonitor(rabbitMqQueue);
//                    break;
//                case QueueType.EasyNetQ:
//                    _jobManager = new EasyNetQJobManager<ElmJobInput>(RabbitMqQueue.HostName, EastNetQQueueName);
//                    _queueMonitor = new RabbitMqMonitor(new RabbitMqQueue(EastNetQQueueName));
//                    break;
//            }
//            _jobManager.MessageEvent += JobManagerMessageEvent;
//            SetProgressBarMax();
//        }

//        private void SetupUi()
//        {
//            _synchronizationContext = SynchronizationContext.Current;
//            threadsTextBox.Text = Environment.ProcessorCount.ToString(CultureInfo.InvariantCulture);
//            queueTypeDropDown.DataSource = Enum.GetNames(typeof(QueueType));
//            queueTypeDropDown.SelectedItem = DefaultQueue;
//        }

//        void JobManagerMessageEvent(MessageEventArgs e)
//        {
//            var threadIdInBrackets = "(Thread: " + e.ThreadId + ")";
//            switch (e.MessageType)
//            {
//                case MessageType.ReceiverStarted:
//                    OutputText("Receiver Started " + threadIdInBrackets);
//                    break;
//                case MessageType.ReceiverEnded:
//                    OutputText("Receiver Ended " + threadIdInBrackets);
//                    break;
//                case MessageType.BatchSent:
//                    OutputText("Batch Sent");
//                    break;
//                case MessageType.WorkFailed:
//                    OutputText("Work failed: " + e.Message);
//                    break;
//                case MessageType.WorkCompleted:
//                    UpdateProgress();
//                    break;
//                case MessageType.GenericMessage:
//                    OutputText(e.Message);
//                    break;
//            }
//        }

//        private void SendMessageClick(object sender, EventArgs e)
//        {
//            var partitionCount = messageCountNumericTextBox.IntValue;
//            var jobName = CreateRandomJobName();
//            var jobList = Enumerable.Range(1, partitionCount).Select(partitionNo => ElmJobInput.Create(partitionNo, jobName));
//            QueueingDatabase.AddJob(jobList.First().JobName, jobList.Count());
//            OutputText("Adding " + partitionCount + " message(s) to the queue...");
//            _jobManager.SendJobs(jobList);
//            SetProgressBarMax();
//        }

//        private static string CreateRandomJobName()
//        {
//            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
//        }

//        private void SetProgressBarMax()
//        {
//            var noMessageInQueue = Convert.ToInt32(_queueMonitor.GetNoMessageInQueue());
//            progressBar.Maximum = noMessageInQueue;
//            progressBar.Value = noMessageInQueue;
//            messagesInQueueLabel.Text = noMessageInQueue.ToString(CultureInfo.InvariantCulture);
//        }

//        private void ReceiveMessageClick(object sender, EventArgs e)
//        {
//            SetProgressBarMax();
//            updateUITimer.Start();
//            var numberOfThreads = threadsTextBox.IntValue;
//            _cancellationTokenSource = new CancellationTokenSource();

//            for (var i = 0; i < numberOfThreads; i++)
//            {
//                Thread.Sleep(10);
//                var elmJob = new ElmJob(OutputText);
//                RunActionOnThread(() => _jobManager.ReceiveMessage(elmJob.DoWork, _cancellationTokenSource));
//            }
//        }

//        private void QueueTypeDropDownSelectedIndexChanged(object sender, EventArgs e)
//        {
//            var queueType = (QueueType)Enum.Parse(typeof(QueueType), queueTypeDropDown.SelectedValue.ToString());
//            SetupQueue(queueType);
//        }

//        private void CancelButtonClick(object sender, EventArgs e)
//        {
//            CancelThreads();
//        }

//        private void CancelThreads()
//        {
//            if (_cancellationTokenSource != null)
//                _cancellationTokenSource.Cancel();
//        }

//        public Task RunActionOnThread(Action action)
//        {
//            return Task.Factory.StartNew(action, _cancellationTokenSource.Token);
//        }

//        private void UpdateProgress()
//        {
//            _synchronizationContext.Post(t =>
//            {
//                var noMessageInQueue = _queueMonitor.GetNoMessageInQueue();
//                if (noMessageInQueue > progressBar.Maximum)
//                    SetProgressBarMax();
//                progressBar.Value = Convert.ToInt32(noMessageInQueue);
//                messagesInQueueLabel.Text = noMessageInQueue.ToString(CultureInfo.InvariantCulture);
//            }, null);
//        }

//        private void OutputText(string text)
//        {
//            _synchronizationContext.Post(t =>
//            {
//                if (outputRichTextBox.Text.Length > 1000)
//                    outputRichTextBox.Clear();
//                outputRichTextBox.AppendText(t.ToString() + Environment.NewLine);
//                outputRichTextBox.ScrollToCaret();
//            }, text);
//        }

//        private void UpdateUiTimerTick(object sender, EventArgs e)
//        {
//            UpdateProgress();
//        }

//    }
//}



