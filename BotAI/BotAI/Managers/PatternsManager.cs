using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BotAI.APIs;
using BotAI.CMD;
using BotAI.Enums;
using Microsoft.CSharp;

namespace BotAI.Managers
{
    public class PatternsManager
    {
        public const string path = "Patterns\\";

        public const string extension = ".cs";

        static Dictionary<string, Type> Scripts = new Dictionary<string, Type>();

        [OnStartUp("Patterns", StartUp.SecondPass)]
        public static void Init() 
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.OutputAssembly = string.Empty;
            parameters.IncludeDebugInformation = false;

            parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");

            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, path)).Where(x => Path.GetExtension(x) == extension).ToArray();

            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, files);

            if (results.Errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError err in results.Errors)
                {
                    sb.AppendLine(string.Format("{0}({1},{2}) : {3}", Path.GetFileName(err.FileName), err.Line, err.Column, err.ErrorText));
                }
                CMDOutput.Write(sb.ToString(), CMDMessage.warrning);
                Console.Read();
                Environment.Exit(0);
            }
            codeProvider.Dispose();

            foreach (var type in results.CompiledAssembly.GetTypes())
            {
                Scripts.Add(type.Name, type);
            }
        }
        public static bool Contains(string name)
        {
            return Scripts.ContainsKey(name);
        }
        public static void Execute(string name)
        {
            if (!Scripts.ContainsKey(name))
            {
                CMDOutput.Write("Unable to execute " + name + extension + ". Script not found.", CMDMessage.warrning);
            }
            else
            {
                PatternScriptManager script = (PatternScriptManager)Activator.CreateInstance(Scripts[name]);
                script.CMD = new CMDApi();
                script.CLIENT = new ClientApi();
                script.GAME = new InGameApi();
                script.Execute();
            }
        }
        public static string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var script in Scripts)
            {
                sb.AppendLine("-" + script.Key);
            }
            return sb.ToString();
        }
    }
}
