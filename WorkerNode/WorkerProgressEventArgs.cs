using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.WorkerNode
{
    public enum ProgressType
    {
        WorkerNodeStarted,
        WorkCompleted,
        WorkStarted,
        WorkFailed
    }

    public class WorkerProgressEventArgs : EventArgs
    {
        public ProgressType MessageType;
        public string Message;
        public int ThreadId;
        public Nullable<Guid> BatchId;
        public int JobId;
    }
}
