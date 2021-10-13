using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EveryItemIs
{
    public class Module : ETGModule
    {
        public static List<int> validItemIDs = new List<int>();
        protected static AutocompletionSettings GiveAutocompletionSettings = new AutocompletionSettings(delegate (string input)
        {
            List<string> list = new List<string>();
            foreach (string text in Game.Items.IDs)
            {
                if (!(ETGMod.Databases.Items[text.Replace("gungeon:", "")] is Gun) && !(ETGMod.Databases.Items[text.Replace("gungeon:", "")] is PlayerItem))
                {
                    bool flag = text.AutocompletionMatch(input.ToLower());
                    if (flag)
                    {
                        Console.WriteLine(string.Format("INPUT {0} KEY {1} MATCH!", input, text));
                        list.Add(text.Replace("gungeon:", ""));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("INPUT {0} KEY {1} NO MATCH!", input, text));
                    }
                }
            }
            return list.ToArray();
        });
        public override void Start()
        {
            try
            {
                ETGModConsole.Commands.AddGroup("everyitemis", delegate (string[] args)
                {
                    ETGModConsole.Log("<color=#ff87ed>Use 'everyitemis add' to add an item to the override pool, or 'everyitemis clear' to return the game to normal.</color>");
                });
                ETGModConsole.Commands.GetGroup("everyitemis").AddUnit("add", new Action<string[]>(HandleAddItem), Module.GiveAutocompletionSettings);
                ETGModConsole.Commands.GetGroup("everyitemis").AddUnit("clear", delegate (string[] args)
                {
                    validItemIDs.Clear();
                    ETGModConsole.Log("<color=#ff87ed>Item replacement disabled, EveryItemIs... Normal.</color>");
                });

                ETGModConsole.Log("<color=#ff87ed>Welcome to EveryItemIs. Use the command 'everyitemis add' to add an item to the override pool.</color>");
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public static void HandleAddItem(string[] args)
        {
            if (args == null || args.Length <= 0)
            {
                if (args.Length > 1) { ETGModConsole.Log("<color=#ff0059>Error, too many arguments.</color>"); }
                else
                {
                    if (ETGMod.Databases.Items[args[0]] != null)
                    {
                        if (!(ETGMod.Databases.Items[args[0]] is Gun) && !(ETGMod.Databases.Items[args[0]] is PlayerItem))
                        {
                            ETGModConsole.Log($"<color=#ff0059>Added {ETGMod.Databases.Items[args[0]].DisplayName} to Override Pool!</color>");
                            validItemIDs.Add(ETGMod.Databases.Items[args[0]].PickupObjectId);
                            string notif = "Every Item Is now";
                            int iterator = 1;
                            foreach (int id in validItemIDs)
                            {
                                if ((iterator == validItemIDs.Count) && validItemIDs.Count > 1)
                                {
                                    if (iterator == 2)
                                    {
                                    notif += $" or {PickupObjectDatabase.GetById(id).DisplayName}.";
                                    }
                                    else
                                    {
                                        notif += $", or {PickupObjectDatabase.GetById(id).DisplayName}.";

                                    }
                                }
                                else if (iterator == 1)
                                {
                                    notif += $" {PickupObjectDatabase.GetById(id).DisplayName}";
                                    if (validItemIDs.Count == 1) notif += ".";
                                }
                                else
                                {
                                    notif += $" {PickupObjectDatabase.GetById(id).DisplayName},";
                                }
                                iterator++;
                            }
                            ETGModConsole.Log($"<color=#ff0059>{notif}</color>");
                        }
                        else
                        {
                            ETGModConsole.Log("<color=#ff0059>EveryItemIs cannot accept guns or actives.</color>");
                        }

                    }
                    else
                    {
                        ETGModConsole.Log("<color=#ff0059>Error, item ID invalid!</color>");
                    }
                }
            }
            else
            {
                ETGModConsole.Log("<color=#ff0059>Please input an item name after 'add'.</color>");
            }
        }
        public override void Exit() { }
        public override void Init() { }
    }
}
