using BotAI.APIs;
using BotAI.Infrastructure;

namespace BotAI.InGame
{
    public class Camera
    {
        public bool Locked
        {
            get;
            set;
        }
        public Camera(InGameApi api)
        {
            this.Locked = false;
        }

        public void toggle()
        {
            InputHelper.LeftClick(1241, 920);
            CMDHelper.DelayInput();
            Locked = !Locked;
        }
        public void lockAlly(int allyIndice)
        {
            string key = "F" + allyIndice;
            InputHelper.KeyUp(key);
            CMDHelper.DelayInput();
            InputHelper.KeyDown(key);
            CMDHelper.DelayInput();
        }
    }
}
