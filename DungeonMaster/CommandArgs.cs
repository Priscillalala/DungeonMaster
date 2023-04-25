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
    public readonly struct CommandArgs
    {

        internal CommandArgs(string[] args)
        {
            this.args = args;
        }

        public string GetString(int index)
        {
            return args[index];
        }

        public int GetInt(int index)
        {
            return int.Parse(args[index]);
        }

        public bool HasString(int index)
        {
            return args.Length > index;
        }

        public bool HasInt(int index)
        {
            return args.Length > index && int.TryParse(args[index], out _);
        }

        public bool TryGetString(int index, out string arg)
        {
            if (args.Length < index)
            {
                arg = args[index];
                return true;
            }
            arg = null;
            return false;
        }

        public bool TryGetInt(int index, out int arg)
        {
            if (args.Length <= index)
            {
                arg = -1;
                return false;
            }
            return int.TryParse(args[index], out arg);
        }

        private readonly string[] args;

    }
}
