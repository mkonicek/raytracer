using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Lucid.Raytracing
{
    [Serializable]
    [XmlInclude(typeof(PointLight))]
    public abstract class Light
    {
        /// <summary>
        /// Strength of the light.
        /// </summary>
        public double Value;

        /// <summary>
        /// Light position.
        /// </summary>
        public Vector Pos;

        /// <summary>
        /// Light color.
        /// </summary>
        public Color Color;

        public Light() { }

        public Light(Vector pos, Color color, double value)
        {
            this.Pos = pos;
            this.Color = color;
            this.Value = value;
        }

        // TODO - compare performance with IEnumerable<Light> GetRaySources() yield
        public abstract Vector GetNextRaySource();

        public abstract int GetRayCount();
    }
}
