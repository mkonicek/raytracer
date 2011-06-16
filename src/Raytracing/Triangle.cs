using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Simple plane given by ax + by + cz + d = 0 used internally by triangle.
    /// </summary>
    [Serializable]
    internal struct BoundingPlane
    {
        internal Vector Normal;
        internal double d;
        /// <summary>
        /// Tells if given vector lies on negative side of this plane.
        /// </summary>
        internal bool liesOnNegativeSide(ref Vector v)
        {
            return (Normal.Dot(v) + d < -Constants.Epsilon);
        }

        /// <summary>
        /// Swaps negative-positive halfspaces.
        /// </summary>
        internal void Invert()
        {
            Normal.Invert();
            d *= -1;
        }
    }

    [Serializable]
    public class Triangle : Primitive
    {
        public Vector[] Vertices;
        /// <summary>
        /// Plane in which the triangle lies.
        /// </summary>
        Plane plane;
        // 3 bounding planes ortogonal to the main plane, to be replaced by wiki link
        BoundingPlane[] bounds;

        public override Vector GetNormalAt(Vector v)
        {
            return plane.Normal/* + new Vector(Math.Sin(v.x * 8) * 0.05, Math.Cos(v.z * 8) * 0.05, 0)*/;
        }

        public override BoundingBox BoundingBox
        {
            get { return box; }
        }

        BoundingBox box = null;
        private BoundingBox createBoundingBox()
        {
            double x = Math.Min(Vertices[0].X, Math.Min(Vertices[1].X, Vertices[2].X));
            double y = Math.Min(Vertices[0].Y, Math.Min(Vertices[1].Y, Vertices[2].Y));
            double z = Math.Min(Vertices[0].Z, Math.Min(Vertices[1].Z, Vertices[2].Z));
            Vector leftUpFront = new Vector(x, y, z);

            x = Math.Max(Vertices[0].X, Math.Max(Vertices[1].X, Vertices[2].X));
            y = Math.Max(Vertices[0].Y, Math.Max(Vertices[1].Y, Vertices[2].Y));
            z = Math.Max(Vertices[0].Z, Math.Max(Vertices[1].Z, Vertices[2].Z));

            return new BoundingBox(leftUpFront - new Vector(Constants.Epsilon, Constants.Epsilon, Constants.Epsilon), 
                new Vector(x + Constants.Epsilon, y + Constants.Epsilon, z + Constants.Epsilon));
        }

        public Triangle() { }

        public Triangle(Material material, params Vector[] vertices)
            : base(material)
        {
            if (vertices.Length != 3)
            {
                throw new ArgumentException(string.Format("Cannot construct triangle from {0} vertices", vertices.Length));
            }
            else
            {
                this.Vertices = new Vector[3];
                vertices.CopyTo(this.Vertices, 0);
                plane = new Plane(vertices, material);
                double test = vertices[0].Dot(plane.Normal) + plane.D;
                test = vertices[1].Dot(plane.Normal) + plane.D;
                test = vertices[2].Dot(plane.Normal) + plane.D;
                double x = test;

                bounds = new BoundingPlane[3];

                Vector n;
                // get bounding plane containing v1, v0
                // normal must be perpendicular to v1 - v0
                n = plane.Normal.Cross(vertices[1] - vertices[0]);
                bounds[0].Normal = n;
                // shift plane so that v0 (thus also v1) lies in the plane
                bounds[0].d = -vertices[0].Dot(n);
                 test = vertices[0].Dot(bounds[0].Normal) + bounds[0].d;
                 test = vertices[1].Dot(bounds[0].Normal) + bounds[0].d;
                // make plane positive side face inside of the triangle
                if (vertices[2].Dot(n) + bounds[0].d < 0)
                    bounds[0].Invert();

                n = plane.Normal.Cross(vertices[2] - vertices[1]);
                bounds[1].Normal = n;
                bounds[1].d = -vertices[1].Dot(n);
                 test = vertices[2].Dot(bounds[1].Normal) + bounds[1].d;
                 test = vertices[1].Dot(bounds[1].Normal) + bounds[1].d;
                if (vertices[0].Dot(n) + bounds[1].d < 0)
                    bounds[1].Invert();

                n = plane.Normal.Cross(vertices[0] - vertices[2]);
                bounds[2].Normal = n;
                bounds[2].d = -vertices[2].Dot(n);
                  test = vertices[0].Dot(bounds[2].Normal) + bounds[2].d;
                  test = vertices[2].Dot(bounds[2].Normal) + bounds[2].d;
                if (vertices[1].Dot(n) + bounds[2].d < 0)
                    bounds[2].Invert();

                double y = test;

                box = createBoundingBox();
            }
        }

        public override double GetIntersection(Ray ray)
        {
            double distance = this.plane.GetIntersection(ray);
            if (distance < Constants.Epsilon)
                return Constants.Infinity;
            Vector isect = Vector.Combine(ray.Origin, 1.0, ray.Direction, distance);

            // insert point into plane equation,
            // test if it lies on negative side -> outside triangle
            if (bounds[0].liesOnNegativeSide(ref isect))
                return Constants.Infinity;
            if (bounds[1].liesOnNegativeSide(ref isect))
                return Constants.Infinity;
            if (bounds[2].liesOnNegativeSide(ref isect))
                return Constants.Infinity;

            return distance;
        }
    }
}
