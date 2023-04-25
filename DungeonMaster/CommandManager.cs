using System;
using BepInEx;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security;
using System.IO;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace DungeonMaster
{
    internal static class CommandManager
    {
        internal static Dictionary<string, Action<CommandArgs>> commands = new Dictionary<string, Action<CommandArgs>>();

        internal static bool Process(string input)
        {
            Debug.LogWarning("Processing input: " + input);
            Queue<string> argsQueue = new Queue<string>(input.Split(null));
            if (commands.TryGetValue(argsQueue.Dequeue(), out Action<CommandArgs> command))
            {
                command.Invoke(new CommandArgs(argsQueue.ToArray()));
                return true;
            }
            return false;
        }

        internal static void Init()
        {
            DungeonMaster.RegisterCommand("item", Item);
            DungeonMaster.RegisterCommand("money", Money);
            DungeonMaster.RegisterCommand("foods", Foods);
            DungeonMaster.RegisterCommand("satiety", Satiety);
        }

        internal static void Item(CommandArgs args)
        {
            string itemName = args.GetString(0);
            Debug.LogWarning("Item name: " + itemName);
            MyItemData[] allItems = MyItemManager.Instance.GetAll();
            MyItemData item = allItems.FirstOrDefault(x => x.name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
                ?? allItems.FirstOrDefault(x => x.name.IndexOf(itemName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            if (!item)
            {
                Debug.LogWarning("did not find item!");
                return;
            }
            ItemOwnInfo itemOwnInfo = new ItemOwnInfo(item.id, ItemOwnInfo.OwnType.NORMAL, ItemOwnInfo.BitrhType.From_Runtime);
            GameManager.Instance.currentPlayer.GetItem(itemOwnInfo);
            /*Vector3 position = GameManager.Instance.currentPlayer.latestAvailablePosition + new Vector3(0f, 1f);
            Item itemInstance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("objects/Item"), position, Quaternion.identity).GetComponent<Item>();
            itemInstance.SetItem(item.id);
            itemInstance.parentID = "DungeonMaster";
            itemInstance.getDelegate = (Action<Item>)Delegate.Combine(itemInstance.getDelegate, new Action<Item>(delegate (Item self)
            {
                if (StageManager.Instance != null && StageManager.Instance.current != null && self.parentID != "")
                {
                    StageManager.Instance.current.GetCurrent().AddObjectData(self.parentID + "_Use", true.ToString());
                    StageManager.Instance.SaveCurrentDungeon();
                }
            }));
            itemInstance._characterController2D.velocity.y = 10f;
            itemInstance.gameObject.AddComponent<AttributionToRoom>();
            Debug.LogWarning("complete");*/
        }

        internal static void Money(CommandArgs args)
        {
            int dif = args.GetInt(0) - GameManager.Instance.currentPlayer.Money;
            if (dif > 0)
            {
                GameManager.Instance.currentPlayer.AddMoney(dif);
            }
            else if (dif < 0)
            {
                GameManager.Instance.currentPlayer.UseMoney(Math.Abs(dif));
            }
        }

        internal static void Foods(CommandArgs args)
        {
            UIManager.Instance.foodBuffPanel.CreateFoodIcons(MyFoodManager.Instance.GetFoodAll().Select(x => new MyFood(x)).ToArray());
            UIManager.Instance.foodBuffPanel.OpenPanel();
            UIManager.Instance.foodBuffPanel.SetShopOwner(null);
        }

        internal static void Satiety(CommandArgs args)
        {
            int dif = args.GetInt(0) - GameManager.Instance.currentPlayer.satiety;
            if (dif > 0)
            {
                GameManager.Instance.currentPlayer.AddSatiety(dif);
            }
            else if (dif < 0)
            {
                GameManager.Instance.currentPlayer.UseSatiety(Math.Abs(dif));
            }
        }
    }
}
