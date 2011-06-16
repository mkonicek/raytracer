using System;
using System.Collections.Generic;
using System.Text;
using Lucid.Base;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Represents model file, used by scn format.
    /// </summary>
    [Serializable]
    public class ModelFileInfo : LucidFileInfo
    {
        /// <summary>
        /// Matrix to transform whole model.
        /// </summary>
        public Matrix TransformMatrix;

        public ModelFileInfo()
        {
        }

        public ModelFileInfo(string fileName, Matrix transformMatrix)
            : base(fileName)
        {
            TransformMatrix = transformMatrix;
        }
    }
}
