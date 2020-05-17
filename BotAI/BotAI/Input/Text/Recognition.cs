﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotAI.Enums;
using BotAI.Image;
using BotAI.Managers;
using Tesseract;

namespace BotAI.Input.Text
{
    public class Recognition
    {
        public const string TESS_PATH = "tessdata/";
        public const string TESS_LANGUAGE = "eng";

        private static Dictionary<string, Point> TextCache = new Dictionary<string, Point>();
        private static Dictionary<string, Point> PhraseCache = new Dictionary<string, Point>();


        private static TesseractEngine Engine;

        [OnStartUp("Text Recognition", StartUp.FifthPass)]
        public static void Initialize()
        {
            Engine = new TesseractEngine(TESS_PATH, TESS_LANGUAGE);
        }

        public static Point TextCoords(string phrase)
        {

            if (Utils.TextTimestampExpired(phrase, 2000))
            {
                ReadText();
                Utils.UpdateTextTimestamp(phrase);
            }

            if (TextCache.ContainsKey(phrase)) 
                return TextCache[phrase];
            if (PhraseCache.ContainsKey(phrase)) 
                return PhraseCache[phrase];

            return new Point(0, 0);
        }

        internal static int GetTextValue(Rectangle rectangle)
        {
            Stopwatch st = Stopwatch.StartNew();

            Bitmap src = Pixels.GetScreenshot();

            Bitmap target = new Bitmap(rectangle.Width, rectangle.Height);


            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), rectangle, GraphicsUnit.Pixel);
            }

            Page page = Engine.Process(target);

            int text;
            string textReplacement;

            textReplacement = page.GetText();
            if (textReplacement.Contains('I'))
                    textReplacement = textReplacement.Replace('I', '1');

            if (textReplacement.Contains(' '))
                    textReplacement = textReplacement.Replace(" ", String.Empty);

            if (textReplacement.Contains('Z'))
                    textReplacement = textReplacement.Replace('Z', '2');

            if (textReplacement.Contains('S'))
                    textReplacement = textReplacement.Replace('S', '5');

            if (textReplacement.Contains("\n"))
                    textReplacement = textReplacement.Replace("\n", String.Empty);

            try
            {
                text = Convert.ToInt32(textReplacement);
            }
            catch
            {
                text = 0;
            }

            src.Dispose();

            target.Dispose();

            page.Dispose();

            return text;
        }

        public static bool TextExists2(Rectangle rectangle, string phrase)
        {
            Stopwatch st = Stopwatch.StartNew();

            Bitmap src = Pixels.GetScreenshot();

            Bitmap target = new Bitmap(rectangle.Width, rectangle.Height);


            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),rectangle, GraphicsUnit.Pixel);
            }

            Page page = Engine.Process(target);

            string text = page.GetText();

            src.Dispose();

            target.Dispose();

            page.Dispose();

            return text.ToLower().Contains(phrase.ToLower());
        }
        public static bool TextExists(string phrase)
        {
            if (Utils.TextTimestampExpired(phrase, 2000))
            {
                ReadText();
                Utils.UpdateTextTimestamp(phrase);
            }

            if (TextCache.ContainsKey(phrase))
            {
                if (TextCache[phrase].X > 0 && TextCache[phrase].Y > 0) return true;

            }
            if (PhraseCache.ContainsKey(phrase))
            {
                if (PhraseCache[phrase].X > 0 && PhraseCache[phrase].Y > 0) 
                    return true;
            }
                return false;
        }
        public static void ReadText()
        {
            Console.WriteLine("Image Preprocessing...");

            List<string> WLines = new List<string>();
            List<Rectangle> WRects = new List<Rectangle>();

            List<string> PLines = new List<string>();
            List<Rectangle> PRects = new List<Rectangle>();

            Bitmap screenshot = Image.Utils.InvertImage(Image.Utils.ContrastImage(Image.Utils.DesaturateImage(Pixels.GetScreenshot()), 25));

            Console.WriteLine("Engine Processing...");
            var data = Engine.Process(screenshot, PageSegMode.SparseText);
            WRects = data.GetSegmentedRegions(PageIteratorLevel.Word);
            PRects = data.GetSegmentedRegions(PageIteratorLevel.TextLine);

            TextCache.Clear();
            PhraseCache.Clear();

            Console.WriteLine("Extracting Words...");

            using (var iterator = data.GetIterator())
            {
                string line = "";
                iterator.Begin();

                do
                {
                    do
                    {
                        do
                        {
                            do
                            {

                                string word = iterator.GetText(PageIteratorLevel.Word).Trim();
                                if (word != "")
                                {
                                    line = line + word + " ";
                                    WLines.Add(word.ToUpper().Trim());
                                }
                            } 
                            while (iterator.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                            if (line != "")
                            {
                                PLines.Add(line.ToUpper().Trim());
                            }
                            line = "";
                        } 
                        while (iterator.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));

                    } 
                    while (iterator.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));

                } 
                while (iterator.Next(PageIteratorLevel.Block));
            }
            data.Dispose();
            screenshot.Dispose();

            Console.WriteLine("Saving results...");

            for (int i = 0; i < WLines.Count; ++i)
            {
                if (!TextCache.ContainsKey(WLines[i]))
                {
                    TextCache.Add(WLines[i],new Point(Convert.ToInt32((WRects[i].X + (WRects[i].Width / 2)) * 1),Convert.ToInt32((WRects[i].Y + (WRects[i].Height / 2)) * 1)));
                }
            }

            for (int i = 0; i < PLines.Count; ++i)
            {
                if (!PhraseCache.ContainsKey(PLines[i]))
                {
                    PhraseCache.Add(PLines[i],new Point(Convert.ToInt32((PRects[i].X + (PRects[i].Width / 2)) * 1),Convert.ToInt32((PRects[i].Y + (PRects[i].Height / 2)) * 1)));
                }
            }
            Console.WriteLine("Read Complete.");
            Console.WriteLine("-----------------------");
        }
    }
}
