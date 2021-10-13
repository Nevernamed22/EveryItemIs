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
                    
                });
                

                ETGModConsole.Log("");
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public override void Exit() { }
        public override void Init() { }
    }
}
