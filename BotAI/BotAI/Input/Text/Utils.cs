using System;
using System.Collections.Generic;

namespace BotAI.Input.Text
{
    public class Utils
    {
        private static Dictionary<string, long> TextTimestamps = new Dictionary<string, long>();

        public static void UpdateTextTimestamp(string phrase)
        {
            TextTimestamps[phrase] = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static bool TextTimestampExpired(string phrase, int step)
        {
            if (!TextTimestamps.ContainsKey(phrase)) TextTimestamps.Add(phrase, 0);

            if (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond > TextTimestamps[phrase] + step)
            {
                return true;
            }
            return false;
        }
    }
}
