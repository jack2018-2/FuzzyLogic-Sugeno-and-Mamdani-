using System.Collections.Generic;
using System.Linq;

namespace Fuzzy
{
    /// <summary>
    /// Объединение нечетких множеств
    /// </summary>
    class Union : List<Term>
    {
        public double GetMaxValue(double x)
        {
            return this.Max(t => t.MembershipFunction(x));
        }

        public double CenterMass(double a, double b)
        {
            var step = 0.01d;
            var sumX = 0.0;
            var sum = 0.0;
            for (double x = a; x <= b; x += step)
            {
                var w = GetMaxValue(x);
                sum += w;
                sumX += x * w;
            }
            return sumX / sum;
        }
    }
}