using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class Result<U>
    {
        public readonly U Data;
        public readonly int Id;
        public readonly Guid BatchId;

        public Result(U data, int id, Guid batchId)
        {
            Data = data;
            Id = id;
            BatchId = batchId;
        }
    }
}
