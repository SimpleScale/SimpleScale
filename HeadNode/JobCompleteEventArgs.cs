using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace SimpleScale.HeadNode
{
    public class JobCompleteEventArgs<U> : EventArgs
    {
        public readonly Result<U> Result;

        public JobCompleteEventArgs(Result<U> result)
        {
            Result = result;
        }
    }
}
