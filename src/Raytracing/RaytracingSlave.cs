using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucid.Base;
using System.Drawing;
using System.IO;
using System.Net;

namespace Lucid.Raytracing
{
    /// <summary>
    /// Concrete worker that knows how to do raytracing.
    /// </summary>
    public class RaytracingSlave : JobSlave
    {
        private Scene scene;

        private string downloadHost;
        /// <summary>
        /// Endpoint from which this worker should download files needed.
        /// </summary>
        public string DownloadHost 
        { 
            get
            {
                return downloadHost;
            }
            set
            {
                downloadHost = value;
                try
                {
                    IPAddress hostAddress = Inv.Common.Net.GetAddressFromHost(value);
                    downloadEndPoint = new IPEndPoint(hostAddress, Lucid.Base.Constants.FilePort);
                }
                catch
                {
                    throw new Exception("Cannot resolve host for file download: " + value);
                }
            }
        }
        public IPEndPoint downloadEndPoint { get; set; }

        public RaytracingSlave()
        {
            this.scene = new Scene();
        }

        public override void SetJob(JobSettings job)
        {
            SceneSettings sceneSettings = job as SceneSettings;
            if (sceneSettings == null)
                throw new Exception("RaytracerWorker expects job of type SceneSettings.");

            this.scene.Settings = sceneSettings;

            // download scene
            string downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scenes");
            FileClient fileClient = new FileClient(downloadPath);
            fileClient.DownloadFilesFrom(this.downloadEndPoint.Address, 
                new LucidFileInfo[] { new LucidFileInfo(sceneSettings.FileName)});

            string sceneFileName = Path.Combine(downloadPath, sceneSettings.FileName);
            ScnFile scnFile = ScnFile.LoadFrom(sceneFileName);

            // download models
            var q = from m in scnFile.ModelFiles
                    select new LucidFileInfo(m.FileName);
            //fileClient = new FileClient(downloadPath);
            fileClient.DownloadFilesFrom(this.downloadEndPoint.Address, q);

            Inv.Log.Log.WriteMessage("Loading scene.");
            // load scene
            scene.LoadFrom(sceneFileName, new ScnFormatLoader());
            scene.Compile();
        }

        public override void RunTask(Task task)
        {
            Rectangle rect = getRectFromTask(task, scene.Settings.ImageWidth, scene.Settings.ImageHeight);
            Color[] resultBuffer = new Color[rect.Width * rect.Height];
            task.Result = resultBuffer;
            scene.Render(rect, (Color[])task.Result);

            // speed test - send random colors
            //int c = 2000;
            //resultBuffer = new Color[c];
            /*for (int i = 0; i < resultBuffer.Length; i++)
            {
                double v = i / (double)resultBuffer.Length;
                resultBuffer[i] = new Color(v, v, v, 1);
            }*/
            //task.Result = resultBuffer;
        }

        private static Point getPointFromNumbering(int pixelNo, int w, int h)
        {
            return new Point(pixelNo % w, pixelNo / w);
        }

        public static Rectangle getRectFromTask(Task task, int w, int h)
        {
            Point leftTop = getPointFromNumbering((int)task.Start, w, h);
            Point rightBottom = getPointFromNumbering((int)task.End, w, h);
            Rectangle result = new Rectangle
                (leftTop.X, leftTop.Y, rightBottom.X - leftTop.X + 1, rightBottom.Y - leftTop.Y + 1);

            return result;
        }
    }
}
