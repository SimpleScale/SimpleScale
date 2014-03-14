using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace SimpleScale.HeadNode
{
    public class JobCompleteEventArgs<ResultU> : EventArgs
    {
        public readonly Result<ResultU> Result;

        public JobCompleteEventArgs(Result<ResultU> result)
        {
            Result = result;
        }
    }
}
