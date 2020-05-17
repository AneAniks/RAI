using System;
using System.Windows.Forms;
using InputManager;

using BotAI.Input;

namespace BotAI.Infrastructure
{
    public class InputHelper
    {
        public static void InputWords(string message, int keyDelay = 50, int delay = 100)
        {
            CMDHelper.Sleep(60);

            foreach (var character in message)
            {
                Keys key = KeyboardInput.GetKey(character);
                Keyboard.KeyPress(key, keyDelay);
                CMDHelper.Sleep(delay);
            }
            CMDHelper.Sleep(60);
        }
        public static void KeyUp(string key)
        {
            Keyboard.KeyUp((Keys)Enum.Parse(typeof(Keys), key));
        }
        public static void KeyDown(string key)
        {
            Keyboard.KeyDown((Keys)Enum.Parse(typeof(Keys), key));
        }
        public static void PressKey(string key)
        {
            Keyboard.KeyPress((Keys)Enum.Parse(typeof(Keys), key), 50);
        }

        public static void MoveMouse(int x, int y)
        {
            InputManager.Mouse.Move(x, y);
        }
        public static void RightClick(int x, int y, int delay = 50)
        {
            InputManager.Mouse.Move(x, y);
            CMDHelper.Sleep(60);
            InputManager.Mouse.PressButton(InputManager.Mouse.MouseKeys.Right, delay);
        }
        public static void LeftClick(int x, int y, int delay = 50)
        {
            InputManager.Mouse.Move(x, y);
            CMDHelper.Sleep(60);
            InputManager.Mouse.PressButton(InputManager.Mouse.MouseKeys.Left, delay);
        }
    }
}
