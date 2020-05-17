using System.Drawing;
using System.Threading;
using InputManager;

using BotAI.Image;


namespace BotAI.Infrastructure
{
    public class ImageHelper
    {
        public static void WaitForImage(string imagePath)
        {
            bool exists = Recognition.ImageExists(imagePath);
            while (!exists)
            {
                exists = Recognition.ImageExists(imagePath);
                Thread.Sleep(1000);
            }
        }

        public static bool ImageExist(string imagePath)
        {
            return Recognition.ImageExists(imagePath);
        }
        public static void LeftClickImage(string image)
        {
            if (Recognition.ImageExists(image))
            {
                Point coords = Recognition.ImageCoords(image);

                Mouse.Move(coords.X, coords.Y);
                Mouse.PressButton(Mouse.MouseKeys.Left, 150);
            }
        }
        public static string GetColor(int x, int y)
        {
            return ColorTranslator.ToHtml(Color.FromArgb(Input.Mouse.GetPixelColor(new Point(x, y)).ToArgb()));
        }
        public static void WaitForColor(int x, int y, string colorHex)
        {
            Color color = Input.Mouse.GetPixelColor(new Point(x, y));

            while (ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb())) != colorHex)
            {
                color = Input.Mouse.GetPixelColor(new Point(x, y));
                Thread.Sleep(1000);
            }
        }
    }
}
