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
    internal class MonitorKeyPressed
    {
        internal MonitorKeyPressed(KeyCode key)
        {
            this.key = key;
        }

        internal bool Update()
        {
            bool keyDown = UnityInput.Current.GetKeyDown(key);
            if (wasKeyDown != keyDown)
            {
                wasKeyDown = keyDown;
                if (keyDown)
                {
                    return true;
                }
            }
            return false;
        }

        private readonly KeyCode key;
        private bool wasKeyDown;

    }
}
