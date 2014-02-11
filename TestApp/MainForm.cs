using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using NLog;

using SimpleScale.HeadNode;
using SimpleScale.Common;
using SimpleScale.WorkerNode;
using SimpleScale.Queues;
using System.Configuration;

namespace TestApp
{
    public partial class MainForm : Form
    {
        private static Logger _logger;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IQueueManager<Member, int> _queueManager;
        private HeadNode<Member, int> _headNode;

        public MainForm()
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
            //_queueManager = new MemoryQueueManager<Member, int>();
            _queueManager = CreateServiceBusQueue();
        }

        private void CreateHeadNode()
        {
            _headNode = new HeadNode<Member, int>(_queueManager);
            _headNode.JobComplete += HeadNodeJobComplete;
        }

        private IQueueManager<Member, int> CreateServiceBusQueue()
        {
            var workQueueName = "Work";
            var workCompletedQueueName = "WorkCompleted";
            var serviceBusConnectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            return new ServiceBusQueueManager<Member, int>(serviceBusConnectionString,
                workQueueName, workCompletedQueueName);
        }

        void HeadNodeJobComplete(object sender, JobCompleteEventArgs<int> e)
        {
            _logger.Info("Job " + e.Result.Id + " complete in batch " + e.Result.BatchId + ".");
        }

        private void StartWorkerNodeButtonClick(object sender, EventArgs e)
        {
            var workerNode = new WorkerNode<Member, int>(_queueManager, new ValueMemberMapJob());
            workerNode.StartAsync(_cancellationTokenSource);
        }

        private void startHeadNodeButton_Click(object sender, EventArgs e)
        {
            var cancelationTokenSource = new CancellationTokenSource();
            _headNode.StartHeadNode(cancelationTokenSource);
            startHeadNodeButton.Enabled = false;
        }

        private void cancelButtonClick(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void addBatchToQueueButtonClick(object sender, EventArgs e)
        {
            _headNode.RunBatch(GetBatch());
        }

        private void logTextBox_TextChanged(object sender, EventArgs e)
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }

        public Batch<Member> GetBatch()
        {
            var jobDataList = new List<Member>{
                CreateMember("Tom"),
                CreateMember("Dick"),
                CreateMember("Harry"),
                CreateMember("Jane"),
                CreateMember("Anne"),
                CreateMember("Bill"),
                CreateMember("Jim"),
                CreateMember("Jill"),
                CreateMember("Mary"),
                CreateMember("Bob"),
                CreateMember("Tom"),
                CreateMember("Dick"),
                CreateMember("Harry"),
                CreateMember("Jane"),
                CreateMember("Anne"),
                CreateMember("Bill"),
                CreateMember("Jim"),
                CreateMember("Jill"),
                CreateMember("Mary"),
                CreateMember("Bob"),
                CreateMember("Tom"),
                CreateMember("Dick"),
                CreateMember("Harry"),
                CreateMember("Jane"),
                CreateMember("Anne"),
                CreateMember("Bill"),
                CreateMember("Jim"),
                CreateMember("Jill"),
                CreateMember("Mary"),
                CreateMember("Bob"),
                CreateMember("Tom"),
                CreateMember("Dick"),
                CreateMember("Harry"),
                CreateMember("Jane"),
                CreateMember("Anne"),
                CreateMember("Bill"),
                CreateMember("Jim"),
                CreateMember("Jill"),
                CreateMember("Mary"),
                CreateMember("Bob")
            };

            return new Batch<Member>(jobDataList);
        }

        public Member CreateMember(string name)
        {
            return new Member { Name = name };
        }
    }
}
