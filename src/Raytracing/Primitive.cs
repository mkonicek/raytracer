using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Lucid.Raytracing
{
    [Serializable]
    [XmlInclude(typeof(Sphere))]	// serialization
	[XmlInclude(typeof(Triangle))]  // serialization
	[XmlInclude(typeof(Plane))]  // serialization
    public abstract class Primitive
    {
        public Material material;
        protected MaterialFunc materialFunc = null;

        public Primitive() { }

        public Primitive(Material material)
        {
            this.material = material;
        }

        public Primitive(MaterialFunc materialFunc)
        {
            this.materialFunc = materialFunc;
        }

        /// <summary>
        /// Finds intersection of this object with given ray.
        /// </summary>
        /// <param name="ray">Ray to intersect with.</param>
        /// <returns>Distance as ray.Direction multiplier, can be negative.</returns>
        public abstract double GetIntersection(Ray ray);

        public abstract BoundingBox BoundingBox
        {
            get;
        }
	

        /// <summary>
        /// Returns surface normal at given point.
        /// </summary>
        /// <param name="v">Point in space.</param>
        public abstract Vector GetNormalAt(Vector v);

        /// <summary>
        /// Returns material at given point.
        /// </summary>
        /// <param name="v">Point in space.</param>
        public Material GetMaterialAt(Vector v)
        {
            if (this.materialFunc == null)
            {
                return this.material;
            }
            else
            {
                return materialFunc(v);
            }
        }
    }
}
