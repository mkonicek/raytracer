using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    public delegate Material MaterialFunc(Vector v);

    [Serializable]
    public class Material
    {
        private Color color;
        private double reflectance;
        private double specular;

        public Material() { }

        public Material(Color color, double reflectance, double specular)
        {
            this.Color = color;
            this.Specular = specular;
            this.Reflectance = reflectance;
        }

        public Color Color
        {
            get { return this.color; }
            set { color = value; }
        }
        public double Reflectance
        {
            get { return this.reflectance; }
            set
            {
                // between 0.0 - 1.0
                this.reflectance = Math.Max(Math.Min(value, 1.0), 0.0);
            }
        }
        public double Specular
        {
            get { return this.specular; }
            set
            {
                // no upper bound
                this.specular = Math.Max(value, 0.0);
            }
        }
    }
}