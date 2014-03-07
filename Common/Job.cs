using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleScale.Common
{
    [DataContract]
    public class Job<T>
    {
        public static int UnknownJobId = -1;

        [DataMember]
        public readonly T Data;
        
        [DataMember]
        public readonly int Id;
        
        [DataMember]
        public readonly Guid BatchId;

        public Job(T data, int id, Guid batchId)
        {
            Data = data;
            Id = id;
            BatchId = batchId;
        }
    }
}
