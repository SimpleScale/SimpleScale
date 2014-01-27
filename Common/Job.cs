using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScale.Common
{
    public class Job<T>
    {
        private int _Id;
        private T _info;
        public Job(int id, T info)
        {
            _Id = id;
            _info = info;
        }

        public int Id { get { return _Id; } }
        public T Info { get { return _info; } }

    }
}
