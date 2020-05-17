using System;
using System.Collections.Generic;
using System.Reflection;

using BotAI.Enums;

namespace BotAI.CMD
{
    public class CMDOutput 
    {
        private const ConsoleColor color1 = ConsoleColor.Cyan;
        private const ConsoleColor color2 = ConsoleColor.DarkCyan;

        private static Dictionary<CMDMessage, ConsoleColor> Colors = new Dictionary<CMDMessage, ConsoleColor>()
        {
            { CMDMessage.info1,           ConsoleColor.Gray },
            { CMDMessage.info2,           ConsoleColor.DarkGray },
            { CMDMessage.info3,           ConsoleColor.White },
            { CMDMessage.succes,          ConsoleColor.Green },
            { CMDMessage.warrning,        ConsoleColor.Yellow },
            { CMDMessage.error ,          ConsoleColor.DarkRed},
            { CMDMessage.error2,          ConsoleColor.Red }
        };

        public static void OnStartUp()
        { 
            WriteColor2("   _____      ___     ________   __ ");
            WriteColor1("  | __  | ___|_ _|   |   <>   | |  |");
            WriteColor2("  | __  || . || |    |   __   | |  |");
            WriteColor1("  |_____||___||_|    |__|  |__| |__|");     
            Console.WriteLine();
            Console.Title = Assembly.GetCallingAssembly().GetName().Name;
        }

        private static void WriteColored(object value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
        }

        public static void Write(object value, CMDMessage state = CMDMessage.info1)
        {
            WriteColored(value, Colors[state]);
        }
        public static void WriteColor1(object value)
        {
            WriteColored(value, color1);
        }

        public static void WriteColor2(object value)
        {
            WriteColored(value, color2);
        }
    }
}
