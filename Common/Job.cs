using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleScale.Common
{
    [DataContract]
    public class Job<InputT>
    {
        public static int UnknownJobId = -1;

        [DataMember]
        public readonly InputT Data;
        
        [DataMember]
        public readonly int Id;
        
        [DataMember]
        public readonly Guid BatchId;

        public Job(InputT data, int id, Guid batchId)
        {
            Data = data;
            Id = id;
            BatchId = batchId;
        }
    }
}
