using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    public interface IPrimitiveStructure
    {
        /// <summary>
        /// Adds a primitive to the structure.
        /// </summary>
        void Add(Primitive p);

        /// <summary>
        /// Gets the structure to initial state.
        /// </summary>
        void Clear();

        /// <summary>
        /// Implement this if you need some extra preprocessing before
        /// the first call to GetClosestIntersection.
        /// </summary>
        void Compile();

        /// <summary>
        /// Returns closest intersecting object and distance of intersection.
        /// 
        /// TODO: compare performance with
        /// Intersection GetClosestIntersection(Ray ray);
        /// 
        /// Intersection[] GetAllIntersections(Ray ray); - for light through translucent surfaces
        /// </summary>
        Primitive GetClosestIntersection(ref Ray ray, out double distance);
    }
}
