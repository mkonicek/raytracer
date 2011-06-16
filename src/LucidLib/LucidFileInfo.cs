using System;
using System.Collections.Generic;
using System.Text;

namespace Lucid.Base
{
    /// <summary>
    /// Simple file info. Filename and size.
    /// </summary>
    [Serializable]
    public class LucidFileInfo
    {
        /// <summary>
        /// Just file name. No path -> all scene files have to be in same dir as scene.
        /// </summary>
        public string FileName;

        public LucidFileInfo()
        {
        }

        public LucidFileInfo(string fileName)
        {
            FileName = fileName;
       }
    }
}
