using System;
using CitizenFX.Core;
using LostAngeles.Shared;
using NLog;

namespace LostAngeles.Server.Core.Commander
{
    public class Commander : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("COMMANDER");
        
        public Commander()
        {
            EventHandlers[ServerEvents.Commander.InitializeClientEvent] += new Action<CitizenFX.Core.Player>(OnInitializeClient);
        }

        private void OnInitializeClient([FromSource] CitizenFX.Core.Player player)
        {
            var isAdmin = AdminHelper.IsAdmin(player);
            TriggerClientEvent(player, ClientEvents.Commander.SetupClientEvent, isAdmin);
        }
    }
}