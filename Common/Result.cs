using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleScale.Common
{
    [DataContract]
    public class Result<ResultU>
    {
        [DataMember]
        public readonly ResultU Data;

        [DataMember]
        public readonly int Id;

        [DataMember]
        public readonly Guid BatchId;

        [DataMember]
        public readonly Exception Exception;

        public Result(ResultU data, int id, Guid batchId, Exception exception)
        {
            Data = data;
            Id = id;
            BatchId = batchId;
            Exception = exception;
        }

        public Result(ResultU data, int id, Guid batchId)
            : this(data, id, batchId, null)
        {
        }

        public bool HasError
        {
            get
            {
                return Exception != null;
            }
        }
    }
}
