using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using NLog;

using SimpleScale.HeadNode;
using SimpleScale.Common;
using SimpleScale.WorkerNode;
using SimpleScale.Queues;

using TestApp.Factors;

namespace TestApp.Factors
{
    public partial class CountFactorsUserControl : UserControl
    {
        private static Logger _logger;

        private const string JobsQueueName = "JobsQueue";
        private const string JobsCompletedQueueName = "JobsCompletedQueue";
        private IQueueManager<int, FactorsCountResult> _queueManager;
        private HeadNode<int, FactorsCountResult> _headNode;

        public CountFactorsUserControl()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _logger = LogManager.GetCurrentClassLogger();

            CreateQueueManger();
            CreateHeadNode();
        }

        private void CreateQueueManger()
        {
            //_queueManager = new MemoryQueueManager<int, FactorsCountResult>();
            //_queueManager = CreateServiceBusQueue();
            _queueManager = CreateRabbitMqQueue();
        }

        private IQueueManager<int, FactorsCountResult> CreateServiceBusQueue()
        {
            var serviceBusConnectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]; 
            return new ServiceBusQueueManager<int, FactorsCountResult>(serviceBusConnectionString,
                JobsQueueName, JobsCompletedQueueName);
        }

        private IQueueManager<int, FactorsCountResult> CreateRabbitMqQueue()
        {
            return new RabbitMqQueueManager<int, FactorsCountResult>("localhost",
                JobsQueueName, JobsCompletedQueueName);
        }

        private void CreateHeadNode()
        {
            _headNode = new HeadNode<int, FactorsCountResult>(_queueManager);
            _headNode.JobComplete += HeadNodeJobComplete;
        }

        void HeadNodeJobComplete(object sender, JobCompleteEventArgs<FactorsCountResult> e)
        {
            _logger.Info(e.Result.Data.Number + " has " + e.Result.Data.Count + " factors.");
        }

        private void StartWorkerNodeButtonClick(object sender, EventArgs e)
        {
            var workerNode = new WorkerNode<int, FactorsCountResult>(_queueManager, new FactorsCountJob());
            workerNode.StartAsync();
            var nodeCount = int.Parse(workerNodesLabel.Text);
            workerNodesLabel.Text = (++nodeCount).ToString();
        }

        private void StartHeadNodeButtonClick(object sender, EventArgs e)
        {
            var cancelationTokenSource = new CancellationTokenSource();
            _headNode.StartHeadNode();
            startHeadNodeButton.Enabled = false;
        }

        private void AddBatchToQueueButtonClick(object sender, EventArgs e)
        {
            _headNode.RunBatch(CreateBatch());
        }

        private void LogTextBoxTextChanged(object sender, EventArgs e)
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }

        public Batch<int> CreateBatch()
        {
            var jobDataList = Enumerable.Range(200000000, 100).ToList();
            return new Batch<int>(jobDataList);
        }
    }
}
