using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace SimpleScale.WorkerNode.UnitTests
{
    public class TimesTwoJobRunner : IJobRunner<int, int>
    {
        public int DoWork(Job<int> job)
        {
            return job.Data * 2;
        }
    }
}
