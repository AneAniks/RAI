﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BotAI.Image
{
    public class Utils
    {
        private static Dictionary<string, long> ImageTimestamps = new Dictionary<string, long>();

        public static void UpdateImageTimestamp(string image)
        {
            ImageTimestamps[image] = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static bool ImageTimestampExpired(string image, int step)
        {
            if (!ImageTimestamps.ContainsKey(image)) ImageTimestamps.Add(image, 0);

            if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond > ImageTimestamps[image] + step)
            {
                return true;
            }
            return false;
        }

        public static Bitmap TakeScreenCapture()
        {
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);

            var gfx = Graphics.FromImage(image);

            gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            gfx.Dispose();

            return image;
        }

        public static Bitmap ScaleImage(System.Drawing.Image image, Double scale)
        {
            var newWidth = Convert.ToInt32(image.Width * scale);
            var newHeight = Convert.ToInt32(image.Height * scale);

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
            }
            return newImage;
        }

        public static Bitmap DesaturateImage(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                   });

                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);

                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }

        public static Bitmap InvertImage(Bitmap original)
        {
            for (int y = 0; (y <= (original.Height - 1)); y++)
            {
                for (int x = 0; (x <= (original.Width - 1)); x++)
                {
                    Color inv = original.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    original.SetPixel(x, y, inv);
                }
            }
            return original;
        }

        public static Bitmap ContrastImage(Bitmap bmp, int threshold)
        {
            var contrast = Math.Pow((100.0 + threshold) / 100.0, 2);

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var oldColor = bmp.GetPixel(x, y);
                    var red = ((((oldColor.R / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    var green = ((((oldColor.G / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    var blue = ((((oldColor.B / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    if (red > 255) red = 255;
                    if (red < 0) red = 0;
                    if (green > 255) green = 255;
                    if (green < 0) green = 0;
                    if (blue > 255) blue = 255;
                    if (blue < 0) blue = 0;

                    var newColor = Color.FromArgb(oldColor.A, (int)red, (int)green, (int)blue);
                    bmp.SetPixel(x, y, newColor);
                }
            }
            return bmp;
        }
    }
}
