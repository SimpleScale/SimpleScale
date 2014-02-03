using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IMapJob<T, U>
    {
        U DoWork(Job<T> job);
    }
}
