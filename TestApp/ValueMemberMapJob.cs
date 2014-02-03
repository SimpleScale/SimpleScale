using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace TestApp
{
    public class ValueMemberMapJob : IMapJob<Member, int>
    {
        public int DoWork(Job<Member> job)
        {
            var total = 0;
            for (int i = 0; i < 50000000; i++)
                total += GetAge(job.Data);
            return total;
        }

        public int GetAge(Member member)
        {
            return member.Age;
        }
    }
}
