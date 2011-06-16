using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Raytracing
{
    public interface ISceneLoader
    {
        /// <summary>
        /// Adds contents of filename to scene.
        /// </summary>
        void AddToSceneFrom(Scene scene, string fileName);
    }
}
