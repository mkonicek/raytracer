using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    [Serializable]
    public class PrimitiveList : IPrimitiveStructure, IEnumerable<Primitive>
    {
        List<Primitive> list = new List<Primitive>();

        #region IPrimitiveStructure Members

        public void Add(Primitive p)
        {
            list.Add(p);
        }

        public void Clear()
        {
            list = new List<Primitive>();
        }

        public void Remove(Primitive p)
        {
            list.Remove(p);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public void Compile()
        {
        }

        public Primitive GetClosestIntersection(ref Ray ray, out double distance)
        {
            double minDist = Constants.Infinity;
            Primitive closestPrimitive = null;

            foreach (Primitive primitive in list)
            {
                double dist = primitive.GetIntersection(ray);
                if (dist > 0 && dist < minDist) 
                {
                    minDist = dist;
                    closestPrimitive = primitive;
                }
            }

            distance = minDist;
            return closestPrimitive;
        }

        #endregion

        #region IEnumerable<Primitive> Members

        public IEnumerator<Primitive> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        public Primitive this[int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}
