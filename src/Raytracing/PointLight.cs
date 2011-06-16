using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    [Serializable]
    public class PointLight : Light
    {
        public PointLight()
        {
        }

        public PointLight(Vector pos, Color color, double value)
            : base(pos, color, value)
        {
            this.Pos = pos;
            this.Color = color;
            this.Value = value;
        }

        public override Vector GetNextRaySource()
        {
            return this.Pos;
        }

        public override int GetRayCount()
        {
            return 1;
        }
    }
}
