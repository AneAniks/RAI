using System;
using System.Drawing;

namespace BotAI.Image
{
    public class Values
    {
        public static int Health()
        {
            int value = Recognition.MatchingXPixels("Game/health.png", 40);

            int total = Pixels.GetWidth("Game/health.png");

            return (int)Math.Round(100d * value / total);
        }

        public static int EnemyCreepHealth()
        {
            int value = Recognition.MatchingXPixels("Game/enemycreephealth.png", 4);

            int total = Pixels.GetWidth("Game/enemycreephealth.png");

            return (int)Math.Round(100d * value / total);
        }
        public static int AllyCreepHealth()
        {
            int value = Recognition.MatchingXPixels("Game/allycreephealth.png", 4);

            int total = Pixels.GetWidth("Game/allycreephealth.png");

            return (int)Math.Round(100d * value / total);
        }
        public static Point AllyCreepPosition()
        {
            Point position = Recognition.FindImagePosition("Game/allycreephealth.png", 4);
            return position;
        }
        public static Point EnemyCreepPosition()
        {
            Point position = Recognition.FindImagePosition("Game/enemycreephealth.png", 4);
            return position;
        }

        public static Point EnemyTowerStructure()
        {
            Point position = Recognition.FindImagePosition("Game/towerstructure.png", 4);
            return position;
        }
        public static Point EnemyTowerStructure2()
        {
            Point position = Recognition.FindImagePosition("Game/towerstructure2.png", 4);
            return position;
        }
        public static Point EnemyTowerStructure3()
        {
            Point position = Recognition.FindImagePosition("Game/towerstructure3.png", 4);
            return position;
        }
        public static Point EnemyTowerStructure4()
        {
            Point position = Recognition.FindImagePosition("Game/towerstructure4.png", 4);
            return position;
        }
        public static int Mana()
        {
            int value = Recognition.MatchingXPixels("Game/mana.png", 40);

            int total = Pixels.GetWidth("Game/mana.png");

            return (int)Math.Round((double)(100 * value) / total);
        }

        internal static Point EnemyChampion()
        {
            Point position = Recognition.FindImagePosition("Game/enemycharacter.png", 4);
            return position;
        }
    }
}
