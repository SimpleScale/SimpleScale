using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.HeadNode
{
    public class BatchProgress
    {
        public int ItemsInBatch;
        public List<int> ListOfCompletedJobs = new List<int>();

        public bool BatchComplete
        {
            get
            {
                return ItemsInBatch == ListOfCompletedJobs.Count;
            }
        }
    }
}
