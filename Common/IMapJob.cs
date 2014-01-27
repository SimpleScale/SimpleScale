using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IMapJob<T>
    {
        void DoWork(Job<T> job);
    }
}
