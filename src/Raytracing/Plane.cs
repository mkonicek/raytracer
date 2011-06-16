using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    [Serializable]
    public class Plane : Primitive
    {
        public Vector Normal;
        public double D;

        public Plane(Vector normal, double d, Material material)
            : base(material)
        {
            this.Normal = normal;
            this.D = d;
        }

        public Plane(Vector normal, double d, MaterialFunc materialFunc)
            : base(materialFunc)
        {
            this.Normal = normal;
            this.D = d;
        }

        private void initFromVertices(Vector[] vertices)
        {
            if (vertices.Length < 3)
            {
                throw new ArgumentException("Cannot construct plane from less than 3 vertices.");
            }

            Vector v1, v2;
            v1 = vertices[1] - vertices[0];
            v2 = vertices[2] - vertices[0];
            this.Normal = v1.Cross(v2);
            this.Normal.Normalize();
            // from plane equation by inserting vertices[0] (or [1] or [2])
            this.D = -vertices[0].Dot(this.Normal);
        }

        public Plane(Vector[] vertices, Material material)
            : base(material)
        {
            initFromVertices(vertices);
        }

        public Plane(Vector[] vertices, MaterialFunc materialFunc)
            : base(materialFunc)
        {
            initFromVertices(vertices);
        }
        
        public override double GetIntersection(Ray ray)
        {
            double dot = Normal.Dot(ray.Direction);
            // parallel to the plane ?
            if (Math.Abs(dot) < Constants.Epsilon)
            {
                return Constants.Infinity;
            }
            else
            {
                // linear equation
                double t = -(Normal.Dot(ray.Origin) + this.D) / dot;
                return t;
            }
        }

        public override Vector GetNormalAt(Vector v)
        {
            return this.Normal;
        }

        public override BoundingBox BoundingBox
        {
            get { return BoundingBox.Infinite; }
        }
    }

}
