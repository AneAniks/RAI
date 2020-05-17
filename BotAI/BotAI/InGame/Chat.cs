using System.Windows.Forms;
using InputManager;

using BotAI.APIs;
using BotAI.Infrastructure;
using BotAI.Input;


namespace BotAI.InGame
{
    public class Chat
    {
        public Chat(InGameApi api)
        {
        }
        public void talkInGame(string message)
        {
            Keyboard.KeyPress(Keys.Enter, 150);

            CMDHelper.Sleep(100);

            foreach (var character in message)
            {
                Keys key = KeyboardInput.GetKey(character);
                Keyboard.KeyPress(key, 150);
                CMDHelper.Sleep(100);
            }
            CMDHelper.DelayInput();
            Keyboard.KeyPress(Keys.Enter, 150);
            CMDHelper.DelayInput();
        }

        public void talkInChampSelect(string message)
        {
            InputHelper.LeftClick(390, 940, 200);

            CMDHelper.Sleep(100);

            foreach (var character in message)
            {
                Keys key = KeyboardInput.GetKey(character);
                Keyboard.KeyPress(key, 150);
                CMDHelper.Sleep(100);
            }
            CMDHelper.DelayInput();
            Keyboard.KeyPress(Keys.Enter, 150);
            CMDHelper.DelayInput();
        }
    }
}
