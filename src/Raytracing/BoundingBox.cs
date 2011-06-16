using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Axis aligned bounding box.
    /// </summary>
    [Serializable]
    public class BoundingBox
    {
        /// <summary>
        /// Unique infinite box used by objects with infinite dimensions.
        /// </summary>
        public static readonly BoundingBox Infinite = new BoundingBox(new Vector(), new Vector());

        /// <summary>
        /// Unique empty box.
        /// </summary>
        public static readonly BoundingBox Zero = new BoundingBox(new Vector(), new Vector());

        public Vector LeftTopFront;
        public Vector RightBottomBack;

        public BoundingBox(Vector leftTopFront, Vector rightBottomBack)
        {
            if (!leftTopFront.SmallerAllCoordsThan(rightBottomBack))
                throw new Exception("Box specified incorrectly");

            this.LeftTopFront = leftTopFront;
            this.RightBottomBack = rightBottomBack;
        }

        /// <summary>
        /// Common for intersect plane[X,Y,Z] routines.
        /// </summary>
        static double intersectPlane(double origin, double direction, double planeShift)
        {
            //double dot = Normal.Dot(ray.Direction);
            // parallel to the plane ?
            if (Math.Abs(direction) < Constants.Epsilon)
            {
                return Constants.Infinity;
            }
            else
            {
                //double t = -(Normal.Dot(ray.Origin) - this.D) / dot;
                double t = (planeShift - origin) / direction;
                return t;
            }
        }

        /// <summary>
        /// Intersects ray with plane with normal in X axis coming through point [x,0,0].
        /// </summary>
        static double intersectPlaneX(Ray ray, double planeX)
        {
            return intersectPlane(ray.Origin.X, ray.Direction.X, planeX);
        }
        static double intersectPlaneY(Ray ray, double planeY)
        {
            return intersectPlane(ray.Origin.Y, ray.Direction.Y, planeY);
        }
        static double intersectPlaneZ(Ray ray, double planeZ)
        {
            return intersectPlane(ray.Origin.Z, ray.Direction.Z, planeZ);
        }

        delegate double PlaneIntersectDelegate(Ray ray, double planeShift);

        /// <summary>
        /// For ray-box intersection.
        /// </summary>
        static PlaneIntersectDelegate[] planeIntersectors = null;
        static BoundingBox()
        {
            lock (typeof(BoundingBox))
            {
                planeIntersectors = new PlaneIntersectDelegate[3];
                planeIntersectors[0] = intersectPlaneX;
                planeIntersectors[1] = intersectPlaneY;
                planeIntersectors[2] = intersectPlaneZ;
            }
        }

        public double GetIntersection(Ray ray)
        {
            // find largest tNear, smallest tFar
            double tNear = -Constants.Infinity;
            double tFar = Constants.Infinity;
            double t1, t2;

            // for x, y, z
            for (int i = 0; i < 3; i++)
            {
                // intersect in one axis
                // TODO t1 = intersectPlane(ref ray, ref this.LeftTopFront, i);
                //lock (typeof(BoundingBox))
                {
                    t1 = planeIntersectors[i](ray, this.LeftTopFront[i]);
                }
                // ray parallel to planes
                if (t1 == Constants.Infinity)
                {
                    //lock (typeof(BoundingBox))
                    {
                        // ray outside the box
                        if (ray.Origin[i] < this.LeftTopFront[i] || ray.Origin[i] > this.RightBottomBack[i])
                            return Constants.Infinity;
                    }
                }
                else
                {
                    //lock (typeof(BoundingBox))
                    {
                        // TODO t2 = intersectPlane(ref ray, ref this.RightBottomBack , i);
                        t2 = planeIntersectors[i](ray, this.RightBottomBack[i]);
                        if (t1 > t2) Utils.Swap<double>(ref t1, ref t2);
                        // want largest near intersection
                        tNear = Math.Max(t1, tNear);
                        // want smallest far intersection
                        tFar = Math.Min(t2, tFar);
                    }
                }
                // box missed
                if (tNear > tFar)
                    return Constants.Infinity;
                // box is behind
                if (tFar < -Constants.Epsilon)
                    return Constants.Infinity;
            }
            
            return tNear;
        }

        public bool Intersects(BoundingBox other)
        {
            if (this == BoundingBox.Infinite || other == BoundingBox.Infinite)
                return true;

            // we are likely to pass all tests and intersect if the boxes are real close
            double e = Constants.Epsilon*10;
            // try to find a separating plane, do it for all 6 planes
            if (this.LeftTopFront.X - e > other.RightBottomBack.X) return false;
            if (this.RightBottomBack.X + e < other.LeftTopFront.X) return false;

            if (this.LeftTopFront.Y - e > other.RightBottomBack.Y) return false;
            if (this.RightBottomBack.Y + e < other.LeftTopFront.Y) return false;

            if (this.LeftTopFront.Z - e > other.RightBottomBack.Z) return false;
            if (this.RightBottomBack.Z + e < other.LeftTopFront.Z) return false;

            // cannot separate -> they intersect
            return true;
        }

        public bool Contains(Vector v)
        {
            return (v.X > LeftTopFront.X && v.X < RightBottomBack.X) &&
                   (v.Y > LeftTopFront.Y && v.Y < RightBottomBack.Y) &&
                   (v.Z > LeftTopFront.X && v.Z < RightBottomBack.Z);
        }

        /// <summary>
        /// Calculates smalles bouding box surrounding all given boxes.
        /// </summary>
        public static BoundingBox Surrounding(List<BoundingBox> boxes)
        {
            if (boxes == null)
                throw new Exception("Cannot callculate bounding box surrounding null boxes");
            if (boxes.Count == 0)
                return BoundingBox.Zero;

            // LINQ
            double minX = Constants.Infinity, minY = Constants.Infinity, minZ = Constants.Infinity;
            foreach (BoundingBox box in boxes)
            {
                if (box.LeftTopFront.X < minX)
                    minX = box.LeftTopFront.X;
                if (box.LeftTopFront.Y < minY)
                    minY = box.LeftTopFront.Y;
                if (box.LeftTopFront.Z < minZ)
                    minZ = box.LeftTopFront.Z;
            }
            minX -= Constants.Epsilon;
            minY -= Constants.Epsilon;
            minZ -= Constants.Epsilon;
            double maxX = -Constants.Infinity, maxY = -Constants.Infinity, maxZ = -Constants.Infinity;
            foreach (BoundingBox box in boxes)
            {
                if (box.RightBottomBack.X > maxX)
                    maxX = box.RightBottomBack.X;
                if (box.RightBottomBack.Y > maxY)
                    maxY = box.RightBottomBack.Y;
                if (box.RightBottomBack.Z > maxZ)
                    maxZ = box.RightBottomBack.Z;
            }
            maxX += Constants.Epsilon;
            maxY += Constants.Epsilon;
            maxZ += Constants.Epsilon;

            return new BoundingBox(
                new Vector(minX, minY, minZ),
                new Vector(maxX, maxY, maxZ));
        }
    }
}
