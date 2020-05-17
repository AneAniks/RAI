using System;
using System.Collections.Generic;
using System.Drawing;

using BotAI.Infrastructure;
using BotAI.Managers;

namespace BotAI
{
    public class InGame : PatternScriptManager
    {
        private Point CastTargetPoint
        {
            get;
            set;
        }

        public override void Execute()
        {
            bot.log("Waiting for League Of Legends process...");

            bot.waitProcessOpen(GAME_PROCESS_NAME);
            bot.log("Champion selected, loading game...");
           
            bot.waitUntilProcessBounds(GAME_PROCESS_NAME, 1030, 797);

            bot.wait(200);

            bot.log("Waiting for game to load.");

            bot.bringProcessToFront(GAME_PROCESS_NAME);
            bot.centerProcess(GAME_PROCESS_NAME);

            game.waitUntilGameStart();

            bot.log("We are in game !");

            bot.bringProcessToFront(GAME_PROCESS_NAME);
            bot.centerProcess(GAME_PROCESS_NAME);

            bot.wait(1000);

            game.detectSide();

            if (game.getSide() == SideEnum.Blue)
            {
                CastTargetPoint = new Point(1084, 398);
                bot.log("We are blue side!");
            }
            else
            {
                CastTargetPoint = new Point(644, 761);
                bot.log("We are red side!");
            }

            bot.wait(1000);

            game.player.setLevel(0);

            game.chat.talkInGame("GLHF");

            Item[] items = {
                        new Item("Spellthief's Edge",400,false,false,0,game.shop.getItemPosition(ShopItemTypeEnum.Early,1)),
                        new Item("Basic Bots", 300, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Early,0)),
                        new Item("Forbidden Idol", 800, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Essential,1)),
                        new Item("Upgraded bots", 1100, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Essential,0)),
                        new Item("Ardent Censer", 2300, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Essential,2)),
                        new Item("Athene's Unholly Grail", 2100, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Offensive,0)),
                        new Item("Void Staff", 2650, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Offensive,1)),
                        new Item("Redemption", 1600, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Early,2)),
                        new Item("Zhonya", 2900, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Offensive,2))
            };

            List<Item> itemsToBuy = new List<Item>(items);

            game.shop.setItemBuild(itemsToBuy);

            game.camera.toggle();

            game.shop.toogle();
            bot.wait(1000);
            game.player.fixItemsInShop();
            bot.wait(1000);
            game.shop.buyItem(1);
            game.shop.toogle();

            bot.wait(20000);

            game.player.moveNearestBotlaneAllyTower();

            while (bot.isProcessOpen(GAME_PROCESS_NAME))
            {
                bot.bringProcessToFront(GAME_PROCESS_NAME);
                bot.centerProcess(GAME_PROCESS_NAME);

                if (game.player.getCharacterLeveled())
                {
                    game.player.increaseLevel();
                    game.player.upSpells();
                }

                //back and buy
                if (game.player.getHealthPercent() <= 50)
                {
                    if (game.player.isThereAnEnemy())
                        game.player.tryCastSpellToCreep(6);
                }

                if (game.player.getHealthPercent() <= 15)
                {
                    //low hp
                    bot.wait(50);
                    game.player.moveNearestBotlaneAllyTower();
                    bot.wait(8000);
                    game.player.backBaseRegenerateAndBuy();
                    //gold.
                    game.shop.toogle();
                    game.shop.tryBuyItem();
                    game.shop.toogle();
                    bot.wait(200);

                    game.player.moveNearestBotlaneAllyTower();
                    bot.wait(6000);

                    game.player.moveNearestBotlaneAllyTower();
                }

                bot.wait(500);


                if (game.player.allyCreepHealth() != 0)
                {
                    if (game.player.isThereAnEnemy())
                    {
                        game.player.processSpellToEnemyChampions();
                        game.player.moveAwayFromEnemy();
                    }
                    else
                    {
                        if (game.player.enemyCreepHealth() != 0)
                        {
                            game.player.processSpellToEnemyCreeps();
                            game.player.moveAwayFromCreep();
                        }
                        bot.wait(500);
                        game.player.allyCreepPosition();

                    }
                }
                else
                {
                    if (game.player.isThereAnEnemy())
                    {
                        game.player.processSpellToEnemyChampions();
                        game.player.moveAwayFromEnemy();
                    }

                    if (game.player.enemyCreepHealth() != 0)
                    {
                        game.player.processSpellToEnemyCreeps();
                        game.player.moveAwayFromCreep();
                    }

                    if (game.player.nearTowerStructure())
                    {
                        game.player.justMoveAway();
                    }
                }
            }
            bot.executePattern("End");
        }
    }
}