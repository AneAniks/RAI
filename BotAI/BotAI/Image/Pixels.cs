using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using BotAI.Enums;
using BotAI.Managers;

namespace BotAI.Image
{
    public class Pixels
    {
        public const string SCREENSHOT_IMAGE_NAME = "screenshot";

        public const string PATH = "Images/";

        public const int STEP = 250;

        public static string[] EXTENSIONS = new string[]
        {
            ".jpg",
            ".png"
        };

        private static Dictionary<string, int[]> ImagePixels = new Dictionary<string, int[]>();
        private static Dictionary<string, int> ImageHeigth = new Dictionary<string, int>();
        private static Dictionary<string, int> ImageWidth = new Dictionary<string, int>();

        private static Bitmap CurrentScreenshot;

        [OnStartUp("Pixel Cache", StartUp.ThirdPass)]
        public static void Initialize()
        {
            string dir = Path.Combine(Environment.CurrentDirectory, PATH);

            foreach (var file in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories))
            {
                if (EXTENSIONS.Contains(Path.GetExtension(file)))
                {
                    string key = file.Replace(dir, string.Empty).Replace("\\", "/");

                    Bitmap image = (Bitmap)Bitmap.FromFile(file);
                    ImagePixels.Add(key, ConvertImage(image));
                    ImageHeigth.Add(key, image.Height);
                    ImageWidth.Add(key, image.Width);
                    image.Dispose();
                }
            }
        }
        public static bool Exists(string key)
        {
            return ImagePixels.ContainsKey(key);
        }

        public static Bitmap GetScreenshot()
        {
            TakeScreenshot();
            return CurrentScreenshot;
        }

        public static int[] GetPixels(string filename)
        {
            if (filename == SCREENSHOT_IMAGE_NAME)
            {
                TakeScreenshot();
            }
            return ImagePixels[filename];
        }

        public static int GetHeight(string filename)
        {
            return ImageHeigth[filename];
        }

        public static int GetWidth(string filename)
        {
            return ImageWidth[filename];
        }

        public static int GetLength(string filename)
        {
            return ImagePixels[filename].Length;

        }

        private static void TakeScreenshot()
        {
            if (Utils.ImageTimestampExpired(SCREENSHOT_IMAGE_NAME, STEP))
            {
                if (CurrentScreenshot != null) CurrentScreenshot.Dispose();

                CurrentScreenshot = Utils.TakeScreenCapture();

                ImagePixels[SCREENSHOT_IMAGE_NAME] = ConvertImage(CurrentScreenshot);

                ImageWidth[SCREENSHOT_IMAGE_NAME] = CurrentScreenshot.Width;
                ImageHeigth[SCREENSHOT_IMAGE_NAME] = CurrentScreenshot.Height;

                Utils.UpdateImageTimestamp(SCREENSHOT_IMAGE_NAME);
            }
        }

        private static int[] ConvertImage(Bitmap image)
        {
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            BitmapData imageData = image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr ptr = imageData.Scan0;

            int bytes = imageData.Stride * image.Height;

            byte[] rgbValues = new byte[bytes];

            Marshal.Copy(ptr, rgbValues, 0, bytes);

            int count = 0;

            int stride = imageData.Stride;

            int[] pixels = new int[image.Width * image.Height];

            for (int column = 0; column < imageData.Height; column++)
            {
                for (int row = 0; row < imageData.Width; row++)
                {
                    pixels[count] = (((rgbValues[(column * stride) + (row * 3) + 2]) & 0xff) << 16) + (((rgbValues[(column * stride) + (row * 3) + 1]) & 0xff) << 8) + ((rgbValues[(column * stride) + (row * 3)]) & 0xff);

                    count++;
                }
            }
            image.UnlockBits(imageData);

            return pixels;
        }
    }
}
