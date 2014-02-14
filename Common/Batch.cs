using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class Batch<T>
    {
        public readonly Guid Id;
        public readonly List<Job<T>> Jobs;

        public Batch(List<T> jobDataList)
        {
            var jobs = jobDataList.Select(CreateJob);
            Id = Guid.NewGuid();
            Jobs = jobs.ToList();
        }

        private Job<T> CreateJob(T jobData, int index)
        {
            return new Job<T>(jobData, index + 1, Id);
        }

    }
}
