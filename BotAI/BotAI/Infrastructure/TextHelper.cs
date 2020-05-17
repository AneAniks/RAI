using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotAI.Input.Text;

namespace BotAI.Infrastructure
{
    public class TextHelper
    {
        public static void WaitForText(int x, int y, int width, int heigth, string text)
        {
            Rectangle rect = new Rectangle(x, y, width, heigth);

            bool exists = Recognition.TextExists2(rect, text);
            while (!exists)
            {
                CMDHelper.Sleep(1000);
                exists = Recognition.TextExists2(rect, text);
            }
        }
        public static bool TextExists(int x, int y, int width, int heigth, string text)
        {
            return Recognition.TextExists2(new Rectangle(x, y, width, heigth), text);
        }

        internal static int GetTextFromImage(int x, int y, int width, int heigth)
        {
            Rectangle rect = new Rectangle(x, y, width, heigth);
            return Recognition.GetTextValue(rect);
        }
    }
}
