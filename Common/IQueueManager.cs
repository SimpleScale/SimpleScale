using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IQueueManager<T>
    {
        void Add(List<Job<T>> jobs);
        Job<T> Read();
    }
}
