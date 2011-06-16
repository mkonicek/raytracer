using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lucid.Raytracing
{
    public class ScnFormatLoader : ISceneLoader
    {
        #region ISceneLoader Members

        public void AddToSceneFrom(Scene scene, string fileName)
        {
            // deserialize
            ScnFile scnFile = ScnFile.LoadFrom(fileName);
            string modelPath = Path.GetDirectoryName(fileName);
            AddToSceneFrom(scene, scnFile, modelPath);
        }

        #endregion

        public void AddToSceneFrom(Scene scene, ScnFile scnFile, string modelPath)
        {
            foreach (Primitive primitive in scnFile.Primitives)
            {
                scene.addObject(primitive);
            }
            foreach (Light light in scnFile.Lights)
            {
                scene.addLight(light);
            }
            // use VRML loader to load external models
            Vrml2FormatLoader vrmlLoader = new Vrml2FormatLoader();
            foreach (ModelFileInfo modelFile in scnFile.ModelFiles)
            {
                // assume model in same folder as xml file
                string modelFileName = Path.Combine(modelPath, modelFile.FileName);

                vrmlLoader.TransformMatrix = modelFile.TransformMatrix;
                vrmlLoader.AddToSceneFrom(scene, modelFileName);
            }
        }
    }
}
