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
            var total = 0.0;
            for (int i = 0; i < 500000000; i++)
                total += GetAge(job.Data);
            return Convert.ToInt32(total);
        }

        public double GetAge(Member member)
        {
            return (member.Age * 0.1) / 2.0;
        }
    }
}
