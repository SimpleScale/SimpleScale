using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IJobRunner<T, U>
    {
        U DoWork(Job<T> job);
    }
}
