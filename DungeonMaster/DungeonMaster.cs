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

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace DungeonMaster
{
    [BepInPlugin("com.groovesalad.DungeonMaster", "DungeonMaster", "1.0.0")]
    public class DungeonMaster : BaseUnityPlugin
    {
        private readonly MonitorKeyPressed openKey = new MonitorKeyPressed(KeyCode.BackQuote);
        private readonly MonitorKeyPressed enterKey = new MonitorKeyPressed(KeyCode.Return);
        private readonly MonitorKeyPressed historyForward = new MonitorKeyPressed(KeyCode.UpArrow);
        private readonly MonitorKeyPressed historyBack = new MonitorKeyPressed(KeyCode.DownArrow);

        private bool consoleOpen;
        private string input;
        private readonly List<string> inputHistory = new List<string>();
        private int historyIndex;
        private GUIStyle fontStyle;
        private AudioClip inputAudio;

        public static void RegisterCommand(string name, Action<CommandArgs> command)
        {
            CommandManager.commands[name] = command;
        }

        private void Awake()
        {
            CommandManager.Init();
            fontStyle = new GUIStyle();
            fontStyle.fontSize = 48;
            fontStyle.normal.textColor = Color.white;
            inputAudio = Resources.Load<GameObject>("chest/NormalChest_Legend").GetComponent<Chest>().openClip;
        }

        private void Update()
        {
            if (openKey.Update())
            {
                ToggleConsole();
            }
            if (enterKey.Update())
            {
                OnEnter();
            }
            if (historyForward.Update() && historyIndex < inputHistory.Count)
            {
                input = inputHistory[historyIndex++];
            }
            if (historyBack.Update() && historyIndex >= 0)
            {
                input = inputHistory[historyIndex--];
            }
        }

        private void ToggleConsole()
        {
            consoleOpen = !consoleOpen;
        }

        private void OnEnter()
        {
            if (CommandManager.Process(input))
            {
                SoundManager.PlayFX(inputAudio, 1f);
            }
            inputHistory.Add(input);
            historyIndex = 0;
            input = string.Empty;
        }

        private void OnGUI()
        {
            if (!consoleOpen)
            {
                return;
            }
            GUI.Box(new Rect(0, 0, Screen.width, 64f), "");
            input = GUI.TextField(new Rect(0, 0, Screen.width, 64f), input, fontStyle);
        }


    }
}
