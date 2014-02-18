﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public interface IJob<T, U>
    {
        U DoWork(Job<T> job);
    }
}
