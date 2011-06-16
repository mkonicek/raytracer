using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lucid.Raytracing
{
    // TODO: big object surrounded by tons of small objects can fall into many many leaf nodes
    // -> make even non-leaf nodes contain primitives if (level > 2 && primitive would fall into too many children)
    [Serializable]
    public class Octree : IPrimitiveStructure
    {
        // if tree too big, traversing can take too much time
        // eg. 3 levels can be slower than 2 levels !

        // seems to give optimal results
        public static readonly int MaxObjectsInNode = 25; 
        /// <summary>
        /// Max number of recursive splits of root (ie. levels sub root).
        /// </summary>
        public static readonly int MaxTreeDepth = 8;

        private bool compiled;

        OctreeNode root;
        /// <summary>
        /// Objects with big bounding boxes are treated separately,
        /// so that they don't mess up the octree.
        /// </summary>
        PrimitiveList bigObjects;

        private void init()
        {
            compiled = false;
            root = new OctreeNode();
            bigObjects = new PrimitiveList();
        }

        public Octree()
        {
            init();
        }

        #region IPrimitiveStructure Members

        public void Add(Primitive p)
        {
            if (compiled)
            {
                // TODO: continuos compilation?
                throw new InvalidOperationException("Cannot add to compiled octree");
            }
            root.Add(p);
        }

        public void Clear()
        {
            init();
        }

        public void Compile()
        {
            if (root.PrimitiveCount == 0)
                // nothing to compile
                return;

            // calc bounding box
            root.boundingBox = surroundingBox(root.primitives);

            bigObjects = new PrimitiveList();
            for (int i = 0; i < root.primitives.Count; i++)
            {
                if (isBigBoundingBox(root.primitives[i].BoundingBox, root.boundingBox))
                {
                    Primitive bigObject = root.primitives[i];
                    bigObjects.Add(bigObject);
                    root.primitives.Remove(bigObject);
                    i--;
                }
            }

            // recalc bounding box
            root.boundingBox = surroundingBox(root.primitives);

            // split root
            root.SplitRecursively(MaxObjectsInNode, 0, MaxTreeDepth);

            compiled = true;
        }

        // returns all leaf nodes hit by the ray
        private void traverse(OctreeNode node, Ray ray, LinkedList<OctreeNode> list)
        {
            if (node.primitives != null)
            {
                list.AddLast(node);
            }
            if (node.childs != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (node.childs[i].boundingBox.GetIntersection(ray) != Constants.Infinity)
                    {
                        traverse(node.childs[i], ray, list);
                    }
                }
            }
        }

        public Primitive GetClosestIntersection(ref Ray ray, out double distance)
        {
            LinkedList<OctreeNode> hitNodes = new LinkedList<OctreeNode>();
            if (root.boundingBox.GetIntersection(ray) != Constants.Infinity)
            {
                traverse(root, ray, hitNodes);
            }
            Primitive foundPrimitive = null;
            double minDist = Constants.Infinity;
            foreach (OctreeNode node in hitNodes)
            {
                double d = 0;
                Primitive p = node.primitives.GetClosestIntersection(ref ray, out d);
                if (d < minDist)
                {
                    minDist = d;
                    foundPrimitive = p;
                }
            }

            // test big objects
            double dBig = 0;
            Primitive pBig = bigObjects.GetClosestIntersection(ref ray, out dBig);
            if (dBig < minDist)
            {
                minDist = dBig;
                foundPrimitive = pBig;
            }

            distance = minDist;
            return foundPrimitive;
        }

        #endregion

        // checker. can be more than items inserted if some fall into multiple nodes
        private int totalPrimitivesIn(OctreeNode node)
        {
            int sum = node.PrimitiveCount;
            if (node.childs != null)
            {
                foreach (OctreeNode child in node.childs)
                {
                    sum += totalPrimitivesIn(child);
                }
            }
            return sum;
        }

        /// <summary>
        /// Returns whether bounding box is relatively large to reference bouding box.
        /// </summary>
        private bool isBigBoundingBox(BoundingBox tested, BoundingBox reference)
        {
            double threshold = 4;

            Vector extent = tested.RightBottomBack - tested.LeftTopFront;
            Vector referenceExtent = reference.RightBottomBack - reference.LeftTopFront;

            // any dimension large enough
            return
                extent.X * threshold > referenceExtent.X ||
                extent.Y * threshold > referenceExtent.Y ||
                extent.Z * threshold > referenceExtent.Z;
        }

        private BoundingBox surroundingBox(PrimitiveList primitives)
        {
            List<BoundingBox> boxes = new List<BoundingBox>(primitives.Count);
            foreach (Primitive primitive in root.primitives)
            {
                boxes.Add(primitive.BoundingBox);
            }
            return BoundingBox.Surrounding(boxes);
        }
    }
}