using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core
{
    public class GameMode : BaseScript
    {
        public GameMode()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;
            
            // test spawn manager
            var position = new SpawnPosition(-1238, -1840, 2.5f, 320);
            SpawnManager.Spawn(position);
        }
    }
}