using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lucid.Raytracing
{
    public static class GraphicsHelper
    {
        /// <summary>
        /// Renders color buffer to given rect in bitmap.
        /// </summary>
        public static void Render(Color[] colorBuffer, Rectangle rect, Bitmap bitmap)
        {
            unsafe
            {
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
        }
    }
}
