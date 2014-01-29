using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class Job<T>
    {
        public readonly T Data;
        public readonly int Id;
        public readonly Guid BatchId;

        public Job(T data, int id, Guid batchId)
        {
            Data = data;
            Id = id;
            BatchId = batchId;
        }
    }
}
