using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    [Serializable]
    public class OctreeNode
    {
        public OctreeNode[] childs = null;
        // this way, modifying bounding box also modifies BoundingBox.Zero !
        //public BoundingBox boundingBox = BoundingBox.Zero;
        public BoundingBox boundingBox = null;

        public PrimitiveList primitives = new PrimitiveList();

        public void Add(Primitive primitive)
        {
            this.primitives.Add(primitive);
        }

        public int PrimitiveCount
        {
            get { return primitives != null ? primitives.Count : 0; }
        }

        /// <summary>
        /// Splits node into a tree.
        /// </summary>
        /// <param name="maxObjectsInNode">Max objects in each tree node.</param>
        /// <param name="depth">Current depth of recursion.</param
        /// <param name="maxDepth">Max recursion depth.</param>
        public void SplitRecursively(int maxObjectsInNode, int depth, int maxDepth)
        {
            // need to split
            if (this.PrimitiveCount > maxObjectsInNode &&
                // allowed to split
                depth < maxDepth)
            {
                // split objects to 8 new children
                this.split();
                // split children
                foreach (OctreeNode child in this.childs)
                {
                    child.SplitRecursively(maxObjectsInNode, depth + 1, maxDepth);
                }
            }
        }

        /// <summary>
        /// Splits node's primitives into 8 new children nodes.
        /// </summary>
        private void split()
        {
            if (boundingBox == null)
                throw new Exception("Cannot split null bounding box");

            try
            {
                this.childs = new OctreeNode[8];
                for (int i = 0; i < 8; i++)
                {
                    childs[i] = new OctreeNode();
                    childs[i].boundingBox = new BoundingBox(new Vector(), new Vector());
                }

                double dx2 = (boundingBox.RightBottomBack.X - boundingBox.LeftTopFront.X) / 2;
                double dy2 = (boundingBox.RightBottomBack.Y - boundingBox.LeftTopFront.Y) / 2;
                double dz2 = (boundingBox.RightBottomBack.Z - boundingBox.LeftTopFront.Z) / 2;
                childs[0].boundingBox.LeftTopFront = boundingBox.LeftTopFront;
                childs[1].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(dx2, 0, 0);
                childs[2].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(0, dy2, 0);
                childs[3].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(dx2, dy2, 0);
                childs[4].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(0, 0, dz2);
                childs[5].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(dx2, 0, dz2);
                childs[6].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(0, dy2, dz2);
                childs[7].boundingBox.LeftTopFront = boundingBox.LeftTopFront + new Vector(dx2, dy2, dz2);
                Vector sizeVector = new Vector(dx2, dy2, dz2);
                // all childs same size
                for (int i = 0; i < 8; i++)
                {
                    childs[i].boundingBox.RightBottomBack =
                        childs[i].boundingBox.LeftTopFront + sizeVector;
                }

                // distribute primitives into children
                foreach (Primitive primitive in primitives)
                {
                    // primitive may fall into multiple children at the same time
                    // ideally it will fall in only 1 child
                    for (int i = 0; i < 8; i++)
                    {
                        if (primitive.BoundingBox.Intersects(childs[i].boundingBox))
                        {
                            childs[i].Add(primitive);
                        }
                    }
                }
                primitives = null;
            }
            catch
            {
                // in case of exception clean up any inconsistencies
                this.childs = null;
                this.boundingBox = BoundingBox.Zero;
                throw;
            }
        }

    }
}
