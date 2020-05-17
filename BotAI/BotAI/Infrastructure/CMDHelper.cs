using System.Threading;

using BotAI.CMD;
using BotAI.Enums;

namespace BotAI.Infrastructure
{
    class CMDHelper
    {
        private const int delay = 150;

        public static void DelayInput()
        {
            Thread.Sleep(delay);
        }

        public static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }

        public static void Log(string msg)
        {
            CMDOutput.Write(msg, CMDMessage.info2);
        }
    }
}
