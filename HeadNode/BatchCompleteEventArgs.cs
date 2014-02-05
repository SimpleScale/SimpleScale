using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.HeadNode
{
    public class BatchCompleteEventArgs : EventArgs
    {
        public readonly Guid BatchID;
        public BatchCompleteEventArgs(Guid batchId)
        {
            BatchID = batchId;
        }
    }
}
