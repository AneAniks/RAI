using System.Collections.Generic;
using System.Drawing;

namespace BotAI.Image
{
    public class Recognition
    {
        public const int STEP = 250;

        private static Dictionary<string, int> ImageXMatches = new Dictionary<string, int>();
        private static Dictionary<string, Point> ImagePositionMatches = new Dictionary<string, Point>();

        public static Point ImageCoords(string image, int resolution = 3)
        {
            if (Utils.ImageTimestampExpired(image, STEP))
            {
                ImagePositionMatches[image] = FindImagePosition(image, resolution);
                Utils.UpdateImageTimestamp(image);
            }
            return ImagePositionMatches[image];
        }

        public static bool ImageExists(string image, int resolution = 3)
        {
            if (Utils.ImageTimestampExpired(image, STEP))
            {
                ImagePositionMatches[image] = FindImagePosition(image, resolution);
                Utils.UpdateImageTimestamp(image);
            }
            Point coords = ImagePositionMatches[image];

            if (coords.X > 0 && coords.Y > 0) return true;

            return false;
        }

        public static int MatchingXPixels(string image, int resolution = 3)
        {

            if (Utils.ImageTimestampExpired(image, STEP))
            {

                ImageXMatches[image] = MatchImageX(image, resolution);
                Utils.UpdateImageTimestamp(image);
            }
            return ImageXMatches[image];
        } 
        private static int MatchImageX(string filename, int resolution = 3)
        {
            int[] pixels = Pixels.GetPixels(Pixels.SCREENSHOT_IMAGE_NAME);
            int[] search = Pixels.GetPixels(filename);

            for (int key = 0; key < pixels.Length; ++key)
            {
                if (pixels[key] == search[0])
                {
                    bool matched = true;
                    for (int i = 1; i < resolution; ++i)
                    {
                        if (pixels[key + i] != search[i]) 
                            matched = false;
                    }

                    if (matched)
                    {
                        int value = 0;

                        for (int i = 0; i < Pixels.GetWidth(filename); ++i)
                        {
                            if (pixels[key + i] == search[i]) 
                                value++;
                        }
                        return value;
                    }
                }
            }
            return 0;
        }
        public static Point FindImagePosition(string filename, int resolution = 3)
        {
            int[] pixels = Pixels.GetPixels(Pixels.SCREENSHOT_IMAGE_NAME);
            int[] search = Pixels.GetPixels(filename);

            int x = 1;
            int y = 1;

            for (int key = 0; key < pixels.Length; ++key)
            {
                if (pixels[key] == search[0])
                {
                    bool matched = true;

                    for (int i = 1; i < resolution; ++i)
                    {
                        if (pixels[key + i] != search[i]) 
                            matched = false;
                    }
                    if (matched)
                    {
                        for (int i = 1; i < resolution; ++i)
                        {
                            if (pixels[key + (Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME) * i)] != search[(Pixels.GetWidth(filename) * i)]) matched = false;
                        }

                        if (matched)
                        {
                            int start = (key) + (Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME) * (Pixels.GetHeight(filename) - 1)) + Pixels.GetWidth(filename);

                            for (int i = 1; i < resolution; ++i)
                            {
                                if (pixels[start - i] != search[(Pixels.GetWidth(filename) * Pixels.GetHeight(filename)) - i]) 
                                    matched = false;
                            }

                            {
                                if (pixels[key + (Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME) * (Pixels.GetHeight(filename) / 2)) + (Pixels.GetWidth(filename) / 2)] == search[(Pixels.GetWidth(filename) * (Pixels.GetHeight(filename) / 2) + (Pixels.GetWidth(filename) / 2))] &&
                                    pixels[key + (Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME) * ((Pixels.GetHeight(filename) / 2) + 1)) + (Pixels.GetWidth(filename) / 2)] == search[(Pixels.GetWidth(filename) * ((Pixels.GetHeight(filename) / 2) + 1) + (Pixels.GetWidth(filename) / 2))] &&
                                    pixels[key + (Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME) * (Pixels.GetHeight(filename) / 2)) + (Pixels.GetWidth(filename) / 2) + 1] == search[(Pixels.GetWidth(filename) * (Pixels.GetHeight(filename) / 2) + (Pixels.GetWidth(filename) / 2)) + 1] &&
                                    pixels[key + (Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME) * ((Pixels.GetHeight(filename) / 2) + 1)) + (Pixels.GetWidth(filename) / 2) + 1] == search[(Pixels.GetWidth(filename) * ((Pixels.GetHeight(filename) / 2) + 1) + (Pixels.GetWidth(filename) / 2)) + 1])
                                {
                                    return new Point(x, y);
                                }
                            }
                        }
                    }
                }

                if (x == Pixels.GetWidth(Pixels.SCREENSHOT_IMAGE_NAME))
                {
                    y++;
                    x = 0;
                }
                x++;
            }
            return new Point(0, 0);
        }
    }
}
