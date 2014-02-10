using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleScale.Common
{
    [DataContract]
    public class Result<U>
    {
        [DataMember]
        public readonly U Data;

        [DataMember]
        public readonly int Id;

        [DataMember]
        public readonly Guid BatchId;

        public Result(U data, int id, Guid batchId)
        {
            Data = data;
            Id = id;
            BatchId = batchId;
        }
    }
}
