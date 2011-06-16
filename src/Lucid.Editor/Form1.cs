using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lucid.Raytracing;
using System.Drawing.Imaging;

namespace Lucid.Editor
{
    public partial class Form1 : Form
    {
        ScnFile scnFile = new ScnFile();
        Scene scene;
        int _partWidth = 100;
        int _partHeight = 100;

        public Form1()
        {
            InitializeComponent();

            // create scene representation
            this.scnFile = Scenes.getSceneCarTest();
        }

        private void render_Click(object sender, EventArgs e)
        {
            load_Click(this, EventArgs.Empty);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap bitmap = pictureBox1.Image as Bitmap;

            scene.Settings = new SceneSettings();
            scene.Settings.ImageWidth = bitmap.Width;
            scene.Settings.ImageHeight = bitmap.Height;
            scene.Settings.FillColor = new Lucid.Raytracing.Color(1, 1, 1, 1);
            scene.Settings.View = scnFile.DefaultCamera.ToSceneView(90, bitmap.Width / (double)bitmap.Height);

            if (bitmap != null)
            {
                DateTime startTime = DateTime.Now;

                int rectW = _partWidth, rectH = _partHeight;
                int px = 0, py = 0;
                while (py < bitmap.Height)
                {
                    px = 0;
                    while (px < bitmap.Width)
                    {
                        Render(scene, bitmap, new Rectangle(px, py, rectW, rectH));
                        this.Refresh();
                        px += rectW;
                    }
                    py += rectH;
                }

                TimeSpan t = DateTime.Now - startTime;
                //lblTime.Text = t.ToString();
                lblTime.Text = (t.Minutes * 60000 + t.Seconds * 1000 + t.Milliseconds) * 0.001 + " s";
                //lblTime.Text += " " + (bitmap.Width * bitmap.Height / t.TotalSeconds).ToString() + " pix/s";
                pictureBox1.Invalidate();

                //lblTime.Text = (sc.missed / 1000).ToString() + " / " + (sc.total / 1000).ToString();
            }
        }

        private unsafe void Render(Scene sc, Bitmap bitmap, Rectangle rect)
        {
            // clip rect to bitmap
            rect = new Rectangle(
                Math.Max(0, rect.Left),
                Math.Max(0, rect.Top),
                Math.Min(bitmap.Width - rect.Left, rect.Width),
                Math.Min(bitmap.Height - rect.Top, rect.Height));

            Lucid.Raytracing.Color[] colorBuffer = new Lucid.Raytracing.Color[rect.Width * rect.Height];
            sc.Render(rect, colorBuffer);

            BitmapData data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            for (int y = 0; y < data.Height; y++)
            {
                int* dataPos = (int*)((int)data.Scan0 + (y * data.Stride));

                for (int x = 0; x < data.Width; x++)
                {
                    *dataPos = colorBuffer[x + rect.Width * y].ToArgb();
                    dataPos++;
                    //bitmap.SetPixel(x, y, col.ToDrawingColor());
                }
            }

            bitmap.UnlockBits(data);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = AppDomain.CurrentDomain.BaseDirectory;
                this.scnFile.SaveTo(folder + textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void load_Click(object sender, EventArgs e)
        {
            this.scene = new Scene();

            // fill scene
            ScnFormatLoader loader = new ScnFormatLoader();
            //loader.AddToSceneFrom(scene, @"D:\__prog__\__Cs\Lucid\scenes\test_chair\chair.xml");
            loader.AddToSceneFrom(scene, this.scnFile, AppDomain.CurrentDomain.BaseDirectory);

            scene.Compile();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lblTime.Text = string.Format("{0}, {1}", e.X, e.Y);
        }

        private void benchmarkSpheres()
        {
            txtTimes.Clear();
            int step = 3;
            int steps = 4;
            int start = 6;
            for (int count = start; count < start + step * steps; count += step)
            {
                this.scnFile = Scenes.getSceneBenchSpheres(count);
                load_Click(this, EventArgs.Empty);   // load
                render_Click(this, EventArgs.Empty);   // render
                txtTimes.Text += count * count * count + ", " + lblTime.Text + Environment.NewLine;
                //Application.DoEvents();
            }
        }

        private void benchmarkTriangles()
        {
            txtTimes.Clear();
            int steps = 6;
            int count2 = 128;
            for (int i = 0; i < steps; i++)
            {
                this.scnFile = Scenes.getSceneBenchTriangles(count2);
                load_Click(this, EventArgs.Empty);   // load
                render_Click(this, EventArgs.Empty);   // render
                txtTimes.Text += count2 + ", " + lblTime.Text + Environment.NewLine;
                //Application.DoEvents();
                count2 *= 2;
            }
        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {
            benchmarkTriangles();
        }
    }
}