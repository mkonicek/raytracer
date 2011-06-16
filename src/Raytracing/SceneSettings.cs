using System;
using System.Collections.Generic;
using System.Text;
using Lucid.Base;
using System.Runtime.Serialization;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Represents rendering settings that change frequently.
    /// </summary>
    [Serializable]
    public class SceneSettings : JobSettings // dependence on Lucid
    {
        /// <summary>
        /// Name of file with the scene (that the client should download).
        /// </summary>
        // TODO: this is needed solely because of Lucid, not for raytracing
        // -> create wrapper project? see http://trac2.assembla.com/LucidBc/wiki/architecture
        public string FileName { get; set; }

        /// <summary>
        /// View of the scene (camera origin etc.)
        /// </summary>
        public SceneView View { get; set; }

        /// <summary>
        /// Image width. In pixels.
        /// </summary>
        public int ImageWidth { get; set; }

        public Color FillColor { get; set; }

        /// <summary>
        /// Image height. In pixels.
        /// </summary>
        public int ImageHeight { get; set; }

        public bool SpecularOn { get; set; }

        public bool TransparencyOn { get; set; }

        public bool MirrorsOn { get; set; }

        public bool AntialiasOn { get; set; }

        public SceneSettings()
        {
            FillColor = new Color(0.1, 0.1, 0.1, 1);
        }

        //public bool SoftShadowsOn { get; set; }

        //public bool ProceduralMaterialsOn { get; set; }
    }
}
