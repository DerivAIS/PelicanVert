using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.Utilities
{
    class PiecewiseIntegral
    {
        public double value(DateTime t1, DateTime t2, SortedDictionary<DateTime, double> func)
        {
            double output = 0;

            DateTime T = t1;

            foreach (KeyValuePair<DateTime, double> kvp in func)
            {
                if (kvp.Key <= t1)
                    continue;
                else if (kvp.Key > t2)
                    break;
                else
                {
                    output += (kvp.Key - T).TotalDays / 360 * kvp.Value;
                    T = kvp.Key;
                }
            }
            return output;
        }

        public double cumulSum(DateTime t, SortedDictionary<DateTime, double> func)
        {
            double output = 0;

            foreach (KeyValuePair<DateTime, double> kvp in func)
            {
                if (kvp.Key > t)
                    break;
                else
                {
                    output += kvp.Value;
                }
            }
            return output;
        }

    }
}
