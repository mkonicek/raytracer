using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lucid.Base;
using System.Net;
using Lucid.Raytracing;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Inv.Log;

namespace Lucid.Server
{
    public partial class MainForm : Form
    {
        private LucidServer lucidServer = null;
        private FileServer fileServer = null;
        private UdpResponder broadcastResponder = null;
        private RaytracingMaster raytracingMaster = null;

        private Camera currentCamera = null;
        public Camera CurrentCamera
        {
            get
            {
                return currentCamera;
            }
            set
            {
                currentCamera = value;
                txtOriginX.Text = currentCamera.Origin.X.ToString();
                txtOriginY.Text = currentCamera.Origin.Y.ToString();
                txtOriginZ.Text = currentCamera.Origin.Z.ToString();
                txtLookAtX.Text = currentCamera.LookAt.X.ToString();
                txtLookAtY.Text = currentCamera.LookAt.Y.ToString();
                txtLookAtZ.Text = currentCamera.LookAt.Z.ToString();
            }
        }
        private SceneSettings sceneSettings = new SceneSettings();
        private string sceneFileName = null;

        Stopwatch stopWatch = new Stopwatch();
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Log.Loggers.Add(new TextBoxLogger(txtLog));

                raytracingMaster = new RaytracingMaster();
                this.lucidServer = new LucidServer(raytracingMaster);
                lucidServer.ClientConnect += lucidServer_ClientConnect;
                lucidServer.ClientDisconnect += lucidServer_ClientDisconnect;

                lucidServer.TaskSentToClient += lucidServer_TaskSentToClient;
                lucidServer.TaskComplete += raytracerMaster_TaskComplete;

                lucidServer.JobComplete += lucidServer_JobComplete;
                lucidServer.Start();

                tslLocalAddress.Text = "Listening at: " + lucidServer.LocalAddress;

                this.broadcastResponder = new UdpResponder(Raytracing.Constants.AppName);
                broadcastResponder.Start();

                this.fileServer = new FileServer(AppDomain.CurrentDomain.BaseDirectory);
                fileServer.Start();
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        void lucidServer_JobComplete(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = true;
                Log.WriteMessage(string.Format("Job completed."));
                tslTime.Text = stopWatch.Elapsed.ToString();
                stopWatch.Stop();
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    CurrentCamera.LookAt.X = double.Parse(txtLookAtX.Text);
                    CurrentCamera.LookAt.Y = double.Parse(txtLookAtY.Text);
                    CurrentCamera.LookAt.Z = double.Parse(txtLookAtZ.Text);
                    CurrentCamera.Origin.X = double.Parse(txtOriginX.Text);
                    CurrentCamera.Origin.Y = double.Parse(txtOriginY.Text);
                    CurrentCamera.Origin.Z = double.Parse(txtOriginZ.Text);
                }
                catch
                {
                    throw new Exception("Camera origin and look at must be floating point XYZ coordinates.");
                }
                Lucid.Raytracing.Color fillColor;
                try
                {
                    fillColor.A = 1;
                    fillColor.R = int.Parse(txtFillColorR.Text) / (double)255;
                    fillColor.G = int.Parse(txtFillColorG.Text) / (double)255;
                    fillColor.B = int.Parse(txtFillColorB.Text) / (double)255;
                    double max = Math.Max(fillColor.R, Math.Max(fillColor.G, fillColor.B));
                    double min = Math.Min(fillColor.R, Math.Min(fillColor.G, fillColor.B));
                    if (min < 0 || max > 1)
                    {
                        throw new Exception("Color out of range");
                    }
                }
                catch
                {
                    throw new Exception("Background color must be 3 integers between 0-255.");
                }
                this.sceneSettings.FillColor = fillColor;
                try
                {
                    raytracingMaster.TaskRectWidth = int.Parse(txtJobWidth.Text);
                    raytracingMaster.TaskRectHeight = int.Parse(txtJobHeight.Text);
                }
                catch
                {
                    throw new Exception("Job size must be 2 integers.");
                }

                pbResult.Image = new Bitmap(pbResult.Width, pbResult.Height);

                this.fileServer.FilePath = Path.GetDirectoryName(this.sceneFileName);

                this.sceneSettings.FileName = Path.GetFileName(this.sceneFileName);
                int imgWidth = this.pbResult.Image.Width;
                int imgHeight = this.pbResult.Image.Height;
                this.sceneSettings.ImageWidth = imgWidth;
                this.sceneSettings.ImageHeight = imgHeight;

                this.sceneSettings.View = this.CurrentCamera.ToSceneView(90, imgWidth / (double)imgHeight);

                this.lucidServer.StartNewJob(this.sceneSettings);
                txtLog.Clear();
                Log.WriteMessage("Job started " + this.sceneFileName);
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        private void lucidServer_ClientConnect(object sender, IPEndPoint clientEndPoint)
        {
            try
            {
                string clientDns = Inv.Common.Net.TryGetHostNameFromAddress(clientEndPoint.Address);
                lsbConnected.Items.Add(clientDns);
                Log.WriteMessage("Client connected: " + clientDns);
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        void lucidServer_ClientDisconnect(object sender, IPEndPoint clientEndPoint)
        {
            try
            {
                string clientDns = Inv.Common.Net.TryGetHostNameFromAddress(clientEndPoint.Address);
                var q = from string item in lsbConnected.Items
                        where item == clientDns
                        select item;
                lsbConnected.Items.Remove(q.First());
                Log.WriteMessage("Client disconnected: " + clientDns);
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        void lucidServer_TaskSentToClient(object sender, Task taskSent)
        {
            try
            {
                //Log.WriteMessage(string.Format("Task sent to client."));
                if (!stopWatch.IsRunning && taskSent != null)
                {
                    // start stopwatch on first task
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        private void raytracerMaster_TaskComplete(object sender, Task task)
        {
            try
            {
                // TODO : make conversion Task <-> rect extension metods in a separate file
                Rectangle rect = RaytracingSlave.getRectFromTask(task, sceneSettings.ImageWidth, sceneSettings.ImageHeight);
                GraphicsHelper.Render((Lucid.Raytracing.Color[])task.Result, rect, (Bitmap)pbResult.Image);
                pbResult.Refresh();
                //Log.WriteMessage("Task complete from client.");
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnOpenScene_Click(object sender, EventArgs e)
        {
            try
            {
                // new scene opened
                OpenFileDialog openDialog = new OpenFileDialog();
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    // remember opened filename
                    this.sceneFileName = openDialog.FileName;
                    // try to load - test if file is ok
                    ScnFile sceneFile = ScnFile.LoadFrom(this.sceneFileName);
                    // remember default camera
                    this.CurrentCamera = sceneFile.DefaultCamera;

                    btnStart.Enabled = true;
                    Log.WriteMessage("Scene opened: " + this.sceneFileName);
                    // TODO: start immediately after opening?
                    //btnStart_Click(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Inv.Common.Exceptions.ExceptionDialog(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pbResult.Image == null)
                return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                pbResult.Image.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}