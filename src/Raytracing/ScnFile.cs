using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Represents data containing the scene geometry, lights, etc. in .scn format.
    /// Used for easy serialization / deserialization of scn format.
    /// </summary>
    [Serializable]
    public class ScnFile
    {
        /// <summary>
        /// List of models needed to render the scene.
        /// </summary>
        public List<ModelFileInfo> ModelFiles = new List<ModelFileInfo>();

        public List<Material> Materials = new List<Material>();

        public List<Primitive> Primitives = new List<Primitive>();

        public List<Light> Lights = new List<Light>();

        /// <summary>
        /// Camera saved with the scene.
        /// </summary>
        public Camera DefaultCamera = new Camera();

        private static NetDataContractSerializer serializer = new NetDataContractSerializer();

        public void SaveTo(string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                /*BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, this);*/
                serializer.Serialize(stream, this);
            }
        }

        public static ScnFile LoadFrom(string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                /*BinaryFormatter b = new BinaryFormatter();
                return b.Deserialize(stream) as ScnFile;*/
                ScnFile result = serializer.Deserialize(stream) as ScnFile;
                if (result == null)
                {
                    throw new Exception("Bad file format: " + fileName);
                }
                return result;
            }
        }
    }
}
