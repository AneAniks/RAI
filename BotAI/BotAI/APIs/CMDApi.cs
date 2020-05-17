using System;
using System.Diagnostics;
using System.Linq;

using BotAI.CMD;
using BotAI.Enums;
using BotAI.Infrastructure;
using BotAI.Input;
using BotAI.Managers;

namespace BotAI.APIs 
{
    public class CMDApi
    {
        public void log(string msg)
        {
            CMDHelper.Log(msg);
        }
        public void wait(int ms)
        {
            CMDHelper.Sleep(ms);
        }
        public void executePattern(string name)
        {
            PatternsManager.Execute(name);
        }
        public void waitUntilProcessBounds(string processName, int boundsX, int boundsY)
        {
            Mouse.RECT rect = new Mouse.RECT();

            int width = 0;
            int height = 0;

            while (width != boundsX && height != boundsY)
            {
                var process = Process.GetProcessesByName(processName).FirstOrDefault();

                if (process == null)
                {
                    throw new Exception("Process " + process + " cannot be found.");
                }

                Mouse.GetWindowRect(process.MainWindowHandle, out rect);

                width = rect.Right - rect.Left;
                height = rect.Bottom - rect.Top;

                CMDHelper.Sleep(1000);

            }
        }
        public bool isProcessOpen(string processName)
        {
            return Process.GetProcessesByName(processName).Length > 0;
        }
        public void centerProcess(string processName)
        {
            if (!Mouse.CenterProcessWindow(processName))
            {
                CMDOutput.Write("Unable to center process: " + processName, CMDMessage.warrning);
            }
        }
        public void bringProcessToFront(string processName)
        {
            if (!Mouse.BringWindowToFront(processName))
            {
                CMDOutput.Write("Unable to bring process to front: " + processName, CMDMessage.warrning);
            }
        }
        public void waitProcessOpen(string processName)
        {
            while (!Mouse.IsProcessOpen(processName))
            {
                CMDHelper.Sleep(1000);
            }
        }
        public void inputWords(string words, int keyDelay = 50, int delay = 100)
        {
            InputHelper.InputWords(words, keyDelay, delay);
        }
    }
}