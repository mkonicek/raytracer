using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    public class Utils
    {
        /// <summary>
        /// Returns square of given number.
        /// </summary>
        public static double Sqr(double x)
        {
            return x * x;
        }

        /// <summary>
        /// Swaps 2 things
        /// </summary>
        public static void Swap<T>(ref T x, ref T y)
        {
            T a;
            a = x;
            x = y;
            y = a;
        }
    }
}
