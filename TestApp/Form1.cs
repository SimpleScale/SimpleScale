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
    public class Member {
        public string Name;
    }

    public class ValueMemberMapJob : IMapJob<Member>
    {
        public void DoWork(Job<Member> job)
        {
            MessageBox.Show("Processing member " + job.Info.Name);
            Thread.Sleep(1000);
            MessageBox.Show("Member " + job.Info.Name + " processed");
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var queueManager = new MemoryQueueManager<Member>();
            var mapService = new MapService<Member>(queueManager, new ValueMemberMapJob());

            queueManager.Add(GetJobs());
            mapService.Start();
        }

        public List<Job<Member>> GetJobs()
        {
            var job1 = new Job<Member>(0, new Member{ Name = "Tom"});
            var job2 = new Job<Member>(1, new Member{ Name = "Dick"});
            var job3 = new Job<Member>(2, new Member{ Name = "Harry"});

            return new List<Job<Member>>{
                job1, job2, job3
            };
        }


    }
}
