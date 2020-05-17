using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BotAI.APIs;

namespace BotAI.Managers
{
    public abstract class PatternScriptManager
    {
        public const string clientName = "LeagueClientUX";

        public const string gameName = "League of Legends"; 

        public CMDApi CMD 
        {
            protected get;
            set;
        }
        public InGameApi GAME
        {
            protected get;
            set;
        }
        public ClientApi CLIENT
        {
            protected get;
            set;
        }

        public abstract void Execute();
    }
}
