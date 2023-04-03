using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neiroher
{
    internal static class MS
    {
        public static float LRelu(float x)
        {
            return x > 0 ? x : x / 10;
        }
        public static float LReluDer(float x)
        {
            return x > 0 ? 1 : 1 / 10;
        }

        public static float Sigmoid(float x)
        {
            return 1 / (1 + MathF.Pow(MathF.E, -x));
        }

        public static float SigmoidDer(float x)
        {
            float s = Sigmoid(x);
            return s*(1 - s);
        }

        public static float SigmoidDerS(float x)
        {
            return x * (1 - x);
        }
    }
}
