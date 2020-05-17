using System.Collections.Generic;
using System.Drawing;

using BotAI.APIs;
using BotAI.CMD;
using BotAI.Enums;
using BotAI.Infrastructure;

namespace BotAI.InGame
{
    public class Shop
    {
        private Dictionary<ShopItems, Point[]> ItemPositions = new Dictionary<ShopItems, Point[]>()
        {
            { ShopItems.StartItems, new Point[]{new Point(580, 330), new Point(740, 330), new Point(940, 330) } },
            { ShopItems.MovementItems, new Point[]{new Point(580, 440),new Point(740, 440), new Point(940, 440) } },
            { ShopItems.AttackItems, new Point[]{new Point(580, 550), new Point(740, 550), new Point(940, 550)} },
            { ShopItems.MagicItems, new Point[]{new Point(580, 660), new Point(740, 660), new Point(940, 660) } },
            { ShopItems.Tools, new Point[]{new Point(580, 660), new Point(740, 660), new Point(940, 660) } },
            { ShopItems.DefenseItems, new Point[]{new Point(580, 770), new Point(740, 770), new Point(940, 770), new Point(940, 770) } },
        };

        public bool Opened
        {
            get;
            set;
        }
        public List<Item> ItemsToBuy = new List<Item>();

        public Shop(InGameApi api) //: base(api)
        {
            this.Opened = false;
        }
        public void toogle()
        {
            InputHelper.PressKey("P");
            CMDHelper.DelayInput();
            Opened = !Opened;
        }
        public Point getItemPosition(ShopItems type, int indice)
        {
            return ItemPositions[type][indice];
        }
        public void setItemBuild(List<Item> items)
        {
            if (ItemsToBuy != null)
                ItemsToBuy.Clear();

            foreach (Item _item in items)
            {
                ItemsToBuy.Add(_item);

                CMDOutput.Write($"Added {_item.name} on items list");
            }
        }

        public int getPlayerGold()
        {
            return TextHelper.GetTextFromImage(767, 828, 118, 34);
        }

        public void tryBuyItem()
        {
            if (ItemsToBuy != null)
            {
                foreach (Item _item in ItemsToBuy)
                {
                    CMDHelper.Sleep(1000);
                    if (_item.cost <= getPlayerGold())
                    {
                        if (_item.got == false)
                        {
                            CMDOutput.Write($"Character bought {_item.name}.");
                            InputHelper.RightClick(_item.point.X, _item.point.Y, 200);
                            _item.got = true;

                            CMDHelper.Sleep(500);
                            CMDOutput.Write($"{getPlayerGold().ToString()} gold remaining.");
                            tryBuyItem();
                            CMDHelper.Sleep(500);
                        }
                    }
                }
            }
        }
        public void buyItem(int indice)
        {
            Point coords = new Point(0, 0);

            switch (indice)
            {
                case 1:
                    coords = new Point(577, 337);
                    break;
                case 2:
                    coords = new Point(782, 336);
                    break;
                case 3:
                    coords = new Point(595, 557);
                    break;
                case 4:
                    coords = new Point(600, 665);
                    break;
                case 5:
                    coords = new Point(760, 540);
                    break;
                default:
                    CMDOutput.Write("Unknown item indice " + indice + ". Skipping", CMDMessage.warrning);
                    return;
            }
            InputHelper.RightClick(coords.X, coords.Y);

            CMDHelper.DelayInput();
        }
    }
}
