using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class BatchDescription
    {
        public readonly Guid Id;
        public int NoOfJobs;

        public BatchDescription(Guid id)
        {
            Id = id;
        }
    }
}
