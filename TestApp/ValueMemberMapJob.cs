using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace TestApp
{
    public class ValueMemberMapJob : IMapJob<Member>
    {
        public void DoWork(Job<Member> job)
        {
            for (int i = 0; i < 50000000; i++)
                DoNothing();
        }

        public void DoNothing()
        {
            int i = 2 + 1;
        }
    }
}
