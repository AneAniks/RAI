using System;
using System.Reflection;

using BotAI.CMD;
using BotAI.Enums;
using BotAI.Managers;

namespace BotAI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CMDOutput.OnStartUp();

            StartManager.Initialize(Assembly.GetExecutingAssembly());

            HandleCommand();

            Console.Read();
        }
        static void HandleCommand()
        {
            CMDOutput.Write("Enter a pattern filename, type 'help' for help.", CMDMessage.info1);

            string line = Console.ReadLine();

            if (line == "help" || !PatternsManager.Contains(line))
            {
                CMDOutput.Write(PatternsManager.ToString());
                HandleCommand();
                return;
            }
            PatternsManager.Execute(line);

            HandleCommand();
        }
    }
}
