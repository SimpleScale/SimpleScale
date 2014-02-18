using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleScale.Common;

namespace TestApp.Factors
{
    public class FactorsCountJob : IJob<int, FactorsCountResult>
    {
        public FactorsCountResult DoWork(Job<int> job)
        {
            var count = CountNoOfFactors(job.Data);
            return new FactorsCountResult { Number = job.Data, Count = count };
        }

        public int CountNoOfFactors(int numberToCheck)
        {
            int factorCount = 2;
            for (int i = 2; i < numberToCheck; i++)
            {
                if (numberToCheck % i == 0)
                    factorCount++;
            }
            return factorCount;
        }
    }
}
