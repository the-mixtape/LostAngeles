using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NLog;

namespace LostAngeles.Server.Core
{
    public class HardCap : BaseScript
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        
        private readonly Dictionary<int, DateTime> _activePlayers = new Dictionary<int, DateTime>();
        private readonly int _maxClients;

        public HardCap()
        {
            EventHandlers["playerConnecting"] +=
                new Action<CitizenFX.Core.Player, string, CallbackDelegate>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<CitizenFX.Core.Player, string>(OnPlayerDropped);
            EventHandlers["HardCap::PlayerActivated"] += new Action<CitizenFX.Core.Player>(OnPlayerActivated);

            _maxClients = API.GetConvarInt("sv_maxclients", 32);
        }

        private void OnPlayerConnecting([FromSource] CitizenFX.Core.Player source, string playerName,
            CallbackDelegate denyWithReason)
        {
            try
            {
                _log.Info($"Connecting: '{source.Name}' (" +
                          $"steam: {source.Identifiers.Where(i => i.Contains("steam")).FirstOrDefault().ToString()} " +
                          $"ip: {source.Identifiers.Where(i => i.Contains("ip")).FirstOrDefault().ToString()}" +
                          $") | Player count {_activePlayers.Count}/{_maxClients}");

                var playerCount = _activePlayers.Count;
                if (playerCount >= _maxClients)
                {
                    denyWithReason?.Invoke($"This server is full with {playerCount}/{_maxClients} players on.");
                    API.CancelEvent();
                    _log.Info($"Player '{source.Name}' dropped. Server is full.");
                }
            }
            catch (Exception ex)
            {
                _log.Error($"PlayerConnecting error: {ex.Message}");
            }
        }

        private void OnPlayerDropped([FromSource] CitizenFX.Core.Player source, string reason)
        {
            try
            {
                int sessionId = Int32.Parse(source.Handle);
                if (_activePlayers.ContainsKey(sessionId))
                {
                    _activePlayers.Remove(sessionId);
                    _log.Info($"Session#{sessionId} dropped: {reason}");
                }
                TriggerClientEvent("playerDropped", source.Handle, reason);
            }
            catch(Exception ex)
            {
                _log.Error($"PlayerDropped error: {ex.Message}");
            }
        }

        private void OnPlayerActivated([FromSource] CitizenFX.Core.Player source)
        {
            try
            {

                var sessionId = Int32.Parse(source.Handle);
                if (_activePlayers.ContainsKey(sessionId)) return;
                _activePlayers.Add(sessionId, DateTime.UtcNow);

                var license = source.Identifiers["license"];
                _log.Info($"Session#{sessionId} activated (player: '{source.Name}').");

                if (string.IsNullOrEmpty(license))
                {
                    API.DropPlayer(source.Handle, "Unknown error!!! Please try again later.");
                    return;
                }
                
                // var (userId, wasCreated) = await Database.Database.GetOrCreateUser(license);
                // if (userId != null)
                // {
                // string joinMessage = wasCreated ? "Joined for first time to server!" : "Joined to server!"; 
                // Debug.WriteLine($"{source.Name}#{userId:0000} - {joinMessage}");
                // return;
                // }
            }
            catch (Exception ex)
            {
                _log.Error($"PlayerActivated error: {ex.Message}");
            }
        }
    }
}