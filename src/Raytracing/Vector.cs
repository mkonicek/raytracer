using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    // TODO : represent natively as array, make x, y, z aliases for v[0], v[1], v[2]
    [Serializable]
    public struct Vector
    {
        public double X;
        public double Y;
        public double Z;

        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Dot product.
        /// </summary>
        public double Dot(Vector v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        /// <summary>
        /// Cross product.
        /// </summary>
        public Vector Cross(Vector v)
        {
            return new Vector(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
        }

        /// <summary>
        /// Normalizes this vector (in-place).
        /// </summary>
        public void Normalize()
        {
            double lenSq = X * X + Y * Y + Z * Z;
            if (lenSq != 0)
            {
                double mult = 1.0 / Math.Sqrt(lenSq);
                X *= mult;
                Y *= mult;
                Z *= mult;
            }
        }

        /// <summary>
        /// Length squared.
        /// </summary>
        public double LenSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public double Len
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// Inverts this vector (in-place).
        /// </summary>
        public void Invert()
        {
            X *= -1.0;
            Y *= -1.0;
            Z *= -1.0;
        }

        /// <summary>
        /// Multiplies this vector by number (in-place).
        /// </summary>
        public void Mul(double m)
        {
            X *= m;
            Y *= m;
            Z *= m;
        }

        /// <summary>
        /// Divides this vector by number (in-place).
        /// </summary>
        public void Div(double d)
        {
            double m = 1.0 / d;
            X *= m;
            Y *= m;
            Z *= m;
        }

        /// <summary>
        /// Returns true if all coords of this vector are smaller than corresponding v coords.
        /// </summary>
        public bool SmallerAllCoordsThan(Vector v)
        {
            return this.X <= v.X && this.Y <= v.Y && this.Z <= v.Z;
        }

        /// <summary>
        /// Returns true if any coord of this vector is smaller than corresponding v coord.
        /// </summary>
        public bool SmallerAnyCoordThan(Vector v)
        {
            return this.X <= v.X || this.Y <= v.Y || this.Z <= v.Z;
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.X, -v.Y, -v.Z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v, double m)
        {
            return new Vector(v.X * m, v.Y * m, v.Z * m);
        }

        public static Vector operator /(Vector v, double d)
        {
            double m = 1.0 / d;
            return new Vector(v.X * m, v.Y * m, v.Z * m);
        }

        public static Vector operator *(double m, Vector v)
        {
            return new Vector(v.X * m, v.Y * m, v.Z * m);
        }

        public static Vector operator /(double d, Vector v)
        {
            double m = 1.0 / d;
            return new Vector(v.X * m, v.Y * m, v.Z * m);
        }

        /// <summary>
        /// Linear combination.
        /// </summary>
        public static Vector Combine(Vector v1, double t1, Vector v2, double t2)
        {
            return new Vector(v1.X * t1 + v2.X * t2, v1.Y * t1 + v2.Y * t2, v1.Z * t1 + v2.Z * t2);
        }

        /// <summary>
        /// Return x, y, z for 0, 1, 2.
        /// </summary>
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new Exception("Invalid vector index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new Exception("Invalid vector index");
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", X, Y, Z);
        }
    }
}
