using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    [Serializable]
    public class Sphere : Primitive
    {
        public double Radius;
        public Vector Position;

        BoundingBox box = null;
        private BoundingBox createBoundingBox()
        {
            return new BoundingBox(
                Position - new Vector(Radius, Radius, Radius),
                Position + new Vector(Radius, Radius, Radius));
        }

        public Sphere()
        {
        }

        public Sphere(Vector position, double radius, Material material) 
            : base(material)
        {
            this.Position = position;
            this.Radius = radius;
            box = createBoundingBox();
        }

        public Sphere(Vector position, double radius, MaterialFunc materialFunc)
            : base(materialFunc)
        {
            this.Position = position;
            this.Radius = radius;
            box = createBoundingBox();
        }

        public override double GetIntersection(Ray ray)
        {
            double t1, t2;
            double A, A2, B, C, D, sqrtD;

            // move ray instead of sphere - same effect
            ray.Origin.X -= this.Position.X; 
            ray.Origin.Y -= this.Position.Y;
            ray.Origin.Z -= this.Position.Z;

            // solve quadratic equation:
            C = Utils.Sqr(ray.Origin.X) + Utils.Sqr(ray.Origin.Y) + Utils.Sqr(ray.Origin.Z) - Utils.Sqr(Radius);
            A = Utils.Sqr(ray.Direction.X) + Utils.Sqr(ray.Direction.Y) + Utils.Sqr(ray.Direction.Z);
            B = 2 * ray.Origin.Dot(ray.Direction);
            D = B * B - 4 * A * C;

            if (D < 0)
            {
                // no solution
                return Constants.Infinity;
            }
            else
            {
                sqrtD = Math.Sqrt(D);
                A2 = 2 * A;
                t1 = (-B + sqrtD) / A2;
                t2 = (-B - sqrtD) / A2;
                t1 = Math.Min(t1, t2);

                return t1;
            }
        }

        public override Vector GetNormalAt(Vector v)
        {
            Vector n = v - this.Position;
            // normalize (divide by radius)
            n.Mul(1.0 / Radius);
            return n/* + new Vector(Math.Sin(v.X*v.Y * 15) * 0.05, Math.Cos(v.Z * 12) * 0.05, Math.Cos(v.Y * 8)*0.08)*/; 
        }

        public override BoundingBox BoundingBox
        {
            get { return box; }
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
