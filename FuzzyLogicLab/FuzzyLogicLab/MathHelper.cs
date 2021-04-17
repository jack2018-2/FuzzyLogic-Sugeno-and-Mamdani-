using System;

namespace Fuzzy
{
    public static class MathHelper
    {
        public static Func<double, double> Gauss(double c, double b)
        {
            return (x) => Math.Pow(Math.E, -((x - b) * (x - b)) / (2 * c * c));
        }
    }
}