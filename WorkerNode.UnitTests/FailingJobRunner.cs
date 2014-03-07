using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace SimpleScale.WorkerNode.UnitTests
{

    public class FailingJobRunner : IJobRunner<int, int>
    {
        public int DoWork(Job<int> job)
        {
            throw new NotImplementedException();
        }
    }
}
