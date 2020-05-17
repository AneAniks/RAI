using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using BotAI.CMD;
using BotAI.Enums;

namespace BotAI.Managers
{
    public class OnStartUp : Attribute
    {
        public StartUp Type
        {
            get;
            set;
        }

        public bool Hiden
        {
            get; 
            set;
        }

        public string Name
        {
            get; 
            set;
        }

        public bool Exit
        {
            get;
            set;
        }

        public OnStartUp(string name, StartUp type, bool exit = true)
        {
            this.Type = type;
            this.Name = name;
            this.Hiden = false;
            this.Exit = exit;
        }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
    public class StartManager
    {
        public static void Initialize(Assembly startupAssembly)
        {
            CMDOutput.WriteColor2("** Checking all states **");

            Stopwatch watch = Stopwatch.StartNew();

            foreach (var pass in Enum.GetValues(typeof(StartUp)))
            {
                foreach (var item in startupAssembly.GetTypes())
                {
                    var methods = item.GetMethods().ToList().FindAll(x => x.GetCustomAttribute(typeof(OnStartUp), false) != null);
                    var attributes = methods.ConvertAll<KeyValuePair<OnStartUp, MethodInfo>>(x => new KeyValuePair<OnStartUp, MethodInfo>(x.GetCustomAttribute(typeof(OnStartUp), false) as OnStartUp, x)).FindAll(x => x.Key.Type == (StartUp)pass); ;

                    foreach (var data in attributes)
                    {
                        if (!data.Key.Hiden)
                        {
                            CMDOutput.Write("(" + pass + ") Loading " + data.Key.Name + " ...", CMDMessage.info1);
                        }

                        if (data.Value.IsStatic)
                        {
                            try
                            {
                                data.Value.Invoke(null, new object[0]);
                            }
                            catch (Exception ex)
                            {
                                if (data.Key.Exit)
                                {
                                    CMDOutput.Write(ex.ToString(), CMDMessage.error);
                                    Console.ReadKey();
                                    Environment.Exit(0);
                                    return;
                                }
                                else
                                {
                                    CMDOutput.Write("Unable to initialize " + data.Key.Name, CMDMessage.warrning);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            CMDOutput.Write(data.Value.Name + " cannot be executed at startup. Invalid signature", CMDMessage.warrning);
                            continue;
                        }
                    }
                }
            }
            watch.Stop();
            CMDOutput.WriteColor2("** Initialisation Complete (" + watch.Elapsed.Seconds + "s) **");
        }
    }
}
