using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

using BotAI.Enums;
using BotAI.Managers;
using BotAI.CMD;

namespace BotAI.Files
{
    public class Config
    {
        public const string DEFAULT_LEAGUE_PATH = @"C:\Riot Games";

        public const string CONFIG_PATH = "config.json";

        public static Config Instance
        {
            get;
            private set;
        }
        public string ClientPath
        {
            get;
            set;
        }
        [OnStartUp("Config", StartUp.Initial)]
        public static bool LoadConfig()
        {
            if (!Initialize())
            {
                string path = DEFAULT_LEAGUE_PATH;

                if (!Directory.Exists(path))
                {
                    var result = MessageBox.Show("Please select the league of legends 'Riot Game' folder.", "Hello", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);

                    if (result == MessageBoxResult.Cancel)
                    {
                        Environment.Exit(0);
                        return false;
                    }
                    FolderBrowserDialog folderOpen = new FolderBrowserDialog();
                    folderOpen.Description = "Please select the league of legends 'Riot Game' folder.";

                    if (folderOpen.ShowDialog() == DialogResult.OK)
                    {
                        path = folderOpen.SelectedPath;
                        string dirName = new DirectoryInfo(path).Name;

                        if (!Directory.Exists(path) || dirName != "Riot Games")
                        {
                            MessageBox.Show("Invalid Directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return LoadConfig();
                        }
                    }
                    else
                        return LoadConfig();

                }

                CreateConfig(path);
                return true;

            }
            else
            {
                return true;
            }
        }

        private static bool Initialize()
        {
            if (File.Exists(CONFIG_PATH))
            {
                try
                {
                    Instance = Json.Deserialize<Config>(File.ReadAllText(CONFIG_PATH));
                    return true;
                }
                catch
                {
                    File.Delete(CONFIG_PATH);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static void CreateConfig(string clientPath)
        {
            Instance = new Config()
            {
                ClientPath = clientPath,
            };

            Save();

            CMDOutput.Write("Configuration file created!", CMDMessage.succes);
        }
        public static void Save()
        {
            File.WriteAllText(CONFIG_PATH, Json.Serialize(Instance));
        }

        private static bool IsValidDofusPath(string path)
        {
            string combined = Path.Combine(path, @"content/maps");
            return Directory.Exists(combined);
        }
    }
}
