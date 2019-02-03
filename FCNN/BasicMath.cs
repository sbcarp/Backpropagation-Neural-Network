using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    static public class BasicMath
    {
        static public double Sigmoid(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }
        static public double RandomFromRange(double start, double end)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            double output = random.NextDouble() * (end - start) + start;
            random = null;
            return output;
        }
    }
}
