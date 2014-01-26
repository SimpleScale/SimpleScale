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

namespace TestApp
{
    public partial class Form1 : Form
    {
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        MemoryQueueManager<Member> _queueManager = new MemoryQueueManager<Member>();
        int _jobCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            var mapService = new MapService<Member>(_queueManager, new ValueMemberMapJob());
            mapService.StartAsync(_cancellationTokenSource);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            _queueManager.Add(GetJobs());
        }
        
        public List<Job<Member>> GetJobs()
        {
            return new List<Job<Member>>{
                CreateMemberJob("Tom"),
                CreateMemberJob("Dick"),
                CreateMemberJob("Harry"),
                CreateMemberJob("Jane"),
                CreateMemberJob("Anne")
            };
        }

        public Job<Member> CreateMemberJob(string name)
        {
            var member = new Member { Name = name };
            return new Job<Member>(_jobCount++, member);
        }

        private void logTextBox_TextChanged(object sender, EventArgs e)
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }
    }

    public class Member {
        public string Name;
    }

    public class ValueMemberMapJob : IMapJob<Member>
    {
        public void DoWork(Job<Member> job)
        {
            for (int i = 0; i < 50000000; i++)
                DoNothing();
        }

        public void DoNothing() {
            int i = 2 + 1;
        }
    }
}
