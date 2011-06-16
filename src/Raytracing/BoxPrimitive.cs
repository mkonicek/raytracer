using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    public class BoxPrimitive : Primitive
    {
        BoundingBox box;
        Vector center;
        Vector vLeft = new Vector(-1, 0, 0);
        Vector vRight = new Vector(1, 0, 0);
        Vector vUp = new Vector(0, -1, 0);
        Vector vDown = new Vector(0, 1, 0);
        Vector vForward = new Vector(0, 0, 1);
        Vector vBack = new Vector(0, 0, -1);

        public BoxPrimitive(Vector leftTopFront, Vector rightBottomBack, Material mat)
            : base(mat)
        {
            box = new BoundingBox(leftTopFront, rightBottomBack);
            center = Vector.Combine(leftTopFront, 1.0, (rightBottomBack - leftTopFront), 0.5);
        }

        public BoxPrimitive(Vector leftTopFront, Vector rightBottomBack, MaterialFunc matFunc)
            : base(matFunc)
        {
            box = new BoundingBox(leftTopFront, rightBottomBack);
            center = Vector.Combine(leftTopFront, 1.0, (rightBottomBack - leftTopFront), 0.5);
        }

        public override double GetIntersection(Ray ray)
        {
            return this.box.GetIntersection(ray);
        }

        public override BoundingBox BoundingBox
        {
            get { return this.box; }
        }

        public override Vector GetNormalAt(Vector v)
        {
            // determine to which side "center to v" points
            Vector outwards = v - center;
            // transform to [1x1x1] cube
            outwards.X /= (box.RightBottomBack.X - box.LeftTopFront.X);
            outwards.Y /= (box.RightBottomBack.Y - box.LeftTopFront.Y);
            outwards.Z /= (box.RightBottomBack.Z - box.LeftTopFront.Z);
            double absX = Math.Abs(outwards.X);
            double absY = Math.Abs(outwards.Y);
            double absZ = Math.Abs(outwards.Z);
            double maxCoord = Math.Max(absX, Math.Max(absY, absZ));
            if (maxCoord == absX)
                return outwards.X < 0 ? vLeft : vRight;
            else if (maxCoord == absY)
                return outwards.Y < 0 ? vUp : vDown;
            else
                return outwards.Z < 0 ? vBack : vForward; 
        }
    }
}
