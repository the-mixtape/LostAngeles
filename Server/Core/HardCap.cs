using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Server.Domain;
using LostAngeles.Server.Repository;
using LostAngeles.Shared;
using NLog;

namespace LostAngeles.Server.Core
{
    public class HardCap : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("HARDCAP");
        public static IBlacklist BlacklistRepo { get; set; }


        private readonly Dictionary<int, DateTime> _activePlayers = new Dictionary<int, DateTime>();
        private readonly int _maxClients;

        public HardCap()
        {
            EventHandlers["playerConnecting"] +=
                new Action<CitizenFX.Core.Player, string, CallbackDelegate>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<CitizenFX.Core.Player, string>(OnPlayerDropped);
            EventHandlers[ServerEvents.HardCap.PlayerActivatedEvent] += new Action<CitizenFX.Core.Player>(OnPlayerActivated);

            _maxClients = API.GetConvarInt("sv_maxclients", 32);
        }

        private void OnPlayerConnecting([FromSource] CitizenFX.Core.Player source, string playerName,
            CallbackDelegate setKickReason)
        {
            var licenseIdentifier = Helper.GetLicense(source);
            Log.Info($"Connecting: '{source.Name}' (" +
                     $"license: {licenseIdentifier}" +
                     $") | Player count {_activePlayers.Count}/{_maxClients}");


            var playerCount = _activePlayers.Count;
            if (playerCount >= _maxClients)
            {
                var reason = $"You have been kicked (Reason: [Full Server])";
                Log.Info($"Player '{source.Name}' dropped. Reason: {reason}.");
                setKickReason(reason);
                API.CancelEvent();
            }
        }

        private void OnPlayerDropped([FromSource] CitizenFX.Core.Player source, string reason)
        {
            int sessionId = Int32.Parse(source.Handle);
            if (_activePlayers.ContainsKey(sessionId))
            {
                _activePlayers.Remove(sessionId);
                Log.Info($"Session#{sessionId} dropped: {reason}");
            }

            TriggerClientEvent("playerDropped", source.Handle, reason);
        }

        private async void OnPlayerActivated([FromSource] CitizenFX.Core.Player source)
        {
            var sessionId = Int32.Parse(source.Handle);
            if (_activePlayers.ContainsKey(sessionId)) return;
            _activePlayers.Add(sessionId, DateTime.UtcNow);

            var licenseIdentifier = source.Identifiers["license"];
            Log.Info($"Session#{sessionId} activated (player: '{source.Name}').");

            if (string.IsNullOrEmpty(licenseIdentifier))
            {
                API.DropPlayer(source.Handle, "Unknown error!!! Please try again later.");
                return;
            }

            Blacklist blacklist = await BlacklistRepo.GetByLicenseAsync(licenseIdentifier);
            if (blacklist != null)
            {
                var reason =
                    $"You have been kicked (Reason: [Banned])! Ban reason: {blacklist.Reason}. If you think this is a bug, please contact the server administration (Identifier: [{licenseIdentifier}]).";
                Log.Info($"Player '{source.Name}' dropped. Reason: {reason}.");
                API.DropPlayer(source.Handle, reason);
            }
        }
    }
}