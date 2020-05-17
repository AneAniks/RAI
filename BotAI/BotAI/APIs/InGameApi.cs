using BotAI.Enums;
using BotAI.Infrastructure;
using BotAI.InGame;

namespace BotAI.APIs
{
    public class InGameApi
    {
        public Shop shop
        {
            get;
            private set;
        }
        public Camera camera
        {
            get;
            private set;
        }
        public Chat chat
        {
            get;
            private set;
        }
        private Sides side
        {
            get;
            set;
        }
        public Player player
        {
            get;
            private set;
        }
        public InGameApi()
        {
            this.shop = new Shop(this);
            this.camera = new Camera(this);
            this.chat = new Chat(this);
            this.player = new Player(this);
        }

        public void waitUntilGameStart()
        {
            ImageHelper.WaitForColor(997, 904, "#00D304");
        }

        public void detectSide()
        {
            this.side = ImageHelper.GetColor(1343, 868) == "#2A768C" ? Sides.Blue : Sides.Red;
        }
        public Sides getSide()
        {
            return this.side;
        }

        public void moveCenterScreen()
        {
            InputHelper.RightClick(886, 521);
            CMDHelper.DelayInput();
        }
    }
}
