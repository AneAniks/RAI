using System.Drawing;
using System.Threading;

using BotAI.APIs;
using BotAI.CMD;
using BotAI.Enums;
using BotAI.Image;
using BotAI.Infrastructure;

namespace BotAI.InGame
{
    public class Player
    {
        private int PlayerLevel;

        public Player(InGameApi api)
        {

        }

        public void waitUntilMinionSpawn()
        {
            //wait calculations
        }

        public void processSpellToEnemyChampions()
        {
            tryCastSpellToEnemyChampion(1);
            tryCastSpellToEnemyChampion(3);
            CMDHelper.Sleep(550);
            tryCastSpellToEnemyChampion(3);
            tryCastSpellToEnemyChampion(4);
        }

        public void processSpellToEnemyCreeps()
        {
            tryCastSpellToEnemyChampion(3);
            CMDHelper.Sleep(550);
            tryCastSpellToEnemyChampion(3);
        }

        public void moveNearestBotlaneAllyTower()
        {
            if (ImageHelper.GetColor(1410, 924) == "#1C4F5D")
            {
                InputHelper.RightClick(1410, 911);
                CMDHelper.DelayInput();
                return;
            }
            if (ImageHelper.GetColor(1387, 834) == "#3592B1")
            {
                InputHelper.RightClick(1410, 911);
                CMDHelper.DelayInput();
                return;
            }
            
            if (ImageHelper.GetColor(1411, 922) == "#2A788D")
            {
                InputHelper.RightClick(1410, 911);
                CMDHelper.DelayInput();
                return;
            }
      
            if (ImageHelper.GetColor(1366, 916) == "#328AA8")
            {
                InputHelper.RightClick(1366, 916);
                CMDHelper.DelayInput();
                return;
            }
           
            if (ImageHelper.GetColor(1335, 917) == "#2C7B92")
            {
                InputHelper.RightClick(1335, 916);
                CMDHelper.DelayInput();
                return;
            }
            InputHelper.RightClick(1308, 905);
            CMDHelper.DelayInput();
        }

        public bool dead()
        {
            return ImageHelper.GetColor(762, 885) == "#C0FCFA";
        }
        public void castSpell(int indice, int x, int y)
        {
            string key = "D" + indice;
            InputHelper.MoveMouse(x, y);
            InputHelper.PressKey(key);
            CMDHelper.DelayInput();
        }
        public void fixItemsInShop()
        {
            if (ImageHelper.ImageExist("Game/toggleshopitems.png"))
            {
                CMDHelper.DelayInput();
                InputHelper.LeftClick(1050, 240, 150);
                CMDHelper.Log("Items in Shop Fixed!");
                CMDHelper.DelayInput();
            }
        }
        public void upgradeSpell(int indice)
        {
            Point coords = new Point();

            switch (indice)
            {
                case 1:
                    coords = new Point(826, 833);
                    break;
                case 2:
                    coords = new Point(875, 833);
                    break;
                case 3:
                    coords = new Point(917, 833);
                    break;
                case 4:
                    coords = new Point(967, 833);
                    break;
                default:
                    CMDOutput.Write("Unknown spell indice :" + indice, CMDMessage.warrning);
                    return;
            }
            InputHelper.LeftClick(coords.X, coords.Y);
            CMDHelper.DelayInput();
        }
        public void setLevel(int level)
        {
            PlayerLevel = level;
        }
        public void increaseLevel()
        {
            PlayerLevel++;
        }
        public int getLevel()
        {
            return PlayerLevel;
        }

        public void upSpells()
        {
            switch (PlayerLevel)
            {
                case 1:
                    upgradeSpell(1); // Q 1
                    break;
                case 2:
                    upgradeSpell(2); // W 1
                    break;
                case 3:
                    upgradeSpell(3); // E 1
                    break;
                case 4:
                    upgradeSpell(1); // Q 2
                    break;
                case 5:
                    upgradeSpell(1); // Q 3
                    break;
                case 6:
                    upgradeSpell(4); // R 1
                    break;
                case 7:
                    upgradeSpell(1); // Q 4
                    break;
                case 8:
                    upgradeSpell(3); // E 2
                    break;
                case 9:
                    upgradeSpell(1); // Q max
                    break;
                case 10:
                    upgradeSpell(3); // E 3
                    break;
                case 11:
                    upgradeSpell(4); // R 2
                    break;
                case 12:
                    upgradeSpell(3); // E 4
                    break;
                case 13:
                    upgradeSpell(3); // E max
                    break;
                case 14:
                    upgradeSpell(2); // W 2
                    break;
                case 15:
                    upgradeSpell(2); // W 3
                    break;
                case 16:
                    upgradeSpell(4); // R max
                    break;
                case 17:
                    upgradeSpell(2); // W 4
                    break;
                case 18:
                    upgradeSpell(2); // W max
                    break;
                default:
                    upgradeSpell(1);
                    upgradeSpell(2);
                    upgradeSpell(3);
                    upgradeSpell(4);
                    break;
            }
        }
        public bool isGettingAttacked()
        {
            int dmg = TextHelper.GetTextFromImage(881, 507, 65, 41);

            if (dmg != 0)
                return true;
            else
                return false;
        }
        public void justMoveAway()
        {
            Point go = Values.EnemyChampion();

            if (go.X == 0 && go.Y == 0)
                return;

            InputHelper.RightClick(780, 600);
            CMDHelper.Sleep(1000);
        }
        public void nothingHereMoveAway()
        {
            InputHelper.RightClick(780, 600);
            CMDHelper.Sleep(1000);
        }
        public bool getCharacterLeveled()
        {
            return TextHelper.TextExists(835, 772, 81, 27, "level up");
        }

        public int getHealthPercent()
        {
            int value = Values.Health();
            return value;
        }
        public int getManaPercent()
        {
            int value = Values.Mana();
            return value;
        }
        public int enemyCreepHealth()
        {
            int value = Values.EnemyCreepHealth();
            return value;
        }
        public int allyCreepHealth()
        {
            int value = Values.AllyCreepHealth();
            return value;
        }
        public void allyCreepPosition()
        {
            Point go = Values.AllyCreepPosition();

            if (go.X == 0 && go.Y == 0)
                return;

            InputHelper.MoveMouse(go.X - 40, go.Y + 135);
            CMDHelper.Sleep(350);
            InputHelper.PressKey("A");
        }
        public bool isThereAnEnemy()
        {
            Point go = Values.EnemyChampion();

            if (go.X == 0 && go.Y == 0)
                return false;

            InputHelper.PressKey("A");
            return true;
        }

        public bool nearTowerStructure()
        {
            Point go = Values.EnemyTowerStructure();
            Point go2 = Values.EnemyTowerStructure();
            Point go3 = Values.EnemyTowerStructure();
            Point go4 = Values.EnemyTowerStructure();

            bool isNear = false;

            if (go.X != 0 && go.Y != 0)
                isNear = true;

            if (go2.X != 0 && go2.Y != 0)
                isNear = true;

            if (go3.X != 0 && go3.Y != 0)
                isNear = true;

            if (go4.X != 0 && go4.Y != 0)
                isNear = true;

            return isNear;
        }

        public void tryCastSpellToCreep(int indice)
        {
            Point go = Values.EnemyCreepPosition();

            if (go.X == 0 && go.Y == 0)
                return;

            InputHelper.MoveMouse(go.X + 28, go.Y + 42);

            string key = "D" + indice;

            InputHelper.PressKey(key.ToString());
            CMDHelper.Sleep(65);
        }

        public void moveAwayFromEnemy()
        {
            Point go = Values.EnemyChampion();

            if (go.X == 0 && go.Y == 0)
                return;

            InputHelper.RightClick(780, 600);
            CMDHelper.Sleep(1100);
            InputHelper.MoveMouse(970, 540);
            InputHelper.PressKey("A");
        }
        public void moveAwayFromCreep()
        {
            Point go = Values.EnemyCreepPosition();

            if (go.X == 0 && go.Y == 0)
                return;

            InputHelper.RightClick(780, 600);
            CMDHelper.Sleep(1100); 
            InputHelper.MoveMouse(970, 540);
            InputHelper.PressKey("A");
        }

        public void tryCastSpellToEnemyChampion(int indice)
        {
            Point go = Values.EnemyChampion();

            if (go.X == 0 && go.Y == 0)
                return;

            InputHelper.MoveMouse(go.X + 39, go.Y + 129);

            string key = "D" + indice;

            InputHelper.PressKey(key.ToString());
            CMDHelper.Sleep(50);
        }

        public void backBaseRegenerateAndBuy()
        {
            InputHelper.PressKey("B");
            Thread.Sleep(11000);
        }

        public int gameMinute()
        {
            int minute = TextHelper.GetTextFromImage(1426, 171, 16, 16);
            CMDHelper.Log("Game is on minute " + minute.ToString());
            return minute;
        }

        public bool tryMoveLightArea(int X, int Y, string color)
        {
            if (ImageHelper.GetColor(X, Y) == color)
            {
                InputHelper.RightClick(X, Y);
                CMDHelper.DelayInput();
                return true;
            }
            return false;
        }
    }
}
