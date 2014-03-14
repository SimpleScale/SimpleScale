using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class Batch<InputT>
    {
        public readonly Guid Id;
        public readonly List<Job<InputT>> Jobs;

        public Batch(List<InputT> jobDataList)
        {
            var jobs = jobDataList.Select(CreateJob);
            Id = Guid.NewGuid();
            Jobs = jobs.ToList();
        }

        private Job<InputT> CreateJob(InputT jobData, int index)
        {
            return new Job<InputT>(jobData, index + 1, Id);
        }

    }
}
