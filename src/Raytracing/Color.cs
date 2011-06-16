using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    [Serializable]
    public struct Color
    {
        /// <summary>
        /// Red, green, blue, aplha. Range 0.0 to 1.0;
        /// </summary>
        public double R, G, B, A;

        public static Color None
        {
            get
            {
                return new Color(0, 0, 0, 0);
            }
        }

        public static Color Black
        {
            get
            {
                return new Color(0, 0, 0, 1.0);
            }
        }

        /// <summary>
        /// Creates new color.
        /// </summary>
        /// <param name="r">Red (0.0 to 1.0)</param>
        /// <param name="g">Green (0.0 to 1.0)</param>
        /// <param name="b">Blue (0.0 to 1.0)</param>
        /// <param name="alpha">Aplha (0.0 to 1.0)</param>
        public Color(double r, double g, double b, double alpha)
        {
            R = r;
            G = g;
            B = b;
            A = alpha;
        }

        public static Color operator +(Color c1, Color c2)
        {
            return Color.Combine(c1, 1.0, c2, 1.0);
        }

        public static Color operator *(Color c, double m)
        {
            return new Color(c.R * m, c.G * m, c.B * m, c.A);
        }

        public static Color operator *(Color c1, Color c2)
        {
            return new Color(c1.R * c2.R, c1.G * c2.G, c1.B * c2.B, c1.A * c2.A);
        }

        public static bool operator ==(Color c1, Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B && c1.A == c2.A;
        }

        public static bool operator !=(Color c1, Color c2)
        {
            return !(c1 == c2);
        }

        /// <summary>
        /// Linear combination.
        /// </summary>
        public static Color Combine(Color c1, double t1, Color c2, double t2)
        {
            return new Color(
                c1.R * t1 + c2.R * t2, 
                c1.G * t1 + c2.G * t2,
                c1.B * t1 + c2.B * t2, 
                c1.A * t1 + c2.A * t2);
        }

        public System.Drawing.Color ToDrawingColor()
        {
            return System.Drawing.Color.FromArgb(
                (int)Math.Max(0, Math.Min((R * 255), 255)),
                (int)Math.Max(0, Math.Min((G * 255), 255)),
                (int)Math.Max(0, Math.Min((B * 255), 255)));
        }

        /// <summary>
        /// Ignores alpha (sets result alpha to 255).
        /// </summary>
        public int ToArgb()
        {
            int r = (int)Math.Max(0, Math.Min((R * 255), 255));
            int g = (int)Math.Max(0, Math.Min((G * 255), 255));
            int b = (int)Math.Max(0, Math.Min((B * 255), 255));
            return 255 << 24 | r << 16 | g << 8 | b;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]({3})", R, G, B, A);
        }
    }
}
