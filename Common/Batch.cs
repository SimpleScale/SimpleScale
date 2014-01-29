using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class Batch<T>
    {
        public readonly Guid Id;
        public readonly List<T> JobDataList;

        public Batch(List<T> jobDataList)
        {
            JobDataList = jobDataList;
            Id = Guid.NewGuid();
        }

    }
}
