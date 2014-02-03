using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using SimpleScale.HeadNode;
using SimpleScale.Common;
using SimpleScale.WorkerNode;

namespace TestApp
{
    public partial class Form1 : Form
    {
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        MemoryQueueManager<Member, int> _queueManager = new MemoryQueueManager<Member, int>();
        HeadNode<Member, int> _headNode;

        public Form1()
        {
            InitializeComponent();
            _headNode = new HeadNode<Member, int>(_queueManager);
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            var workerNode = new WorkerNode<Member, int>(_queueManager, new ValueMemberMapJob());
            workerNode.StartAsync(_cancellationTokenSource);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void addButton_Click(object sender, EventArgs e)
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
                CreateMember("Anne")
            };

            return new Batch<Member>(jobDataList);
        }

        public Member CreateMember(string name)
        {
            return new Member { Name = name };
        }
    }
}
