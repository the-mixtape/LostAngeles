using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NLog;

namespace LostAngeles.Server.Core
{
    public class SessionManager : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("SESSIONMANAGER");

        private string _currentlyHosting = null;
        private readonly List<Action> _hostReleaseCallbacks = new List<Action>();

        public SessionManager()
        {
            EventHandlers["hostingSession"] += new Action<Player>(OnHostingSession);
            EventHandlers["hostedSession"] += new Action<Player>(OnHostedSession);

            API.EnableEnhancedHostSupport(true);
            Log.Debug("SessionManager initialized");
        }

        private void OnHostingSession([FromSource] Player source)
        {
            if (!String.IsNullOrEmpty(_currentlyHosting))
            {
                TriggerClientEvent(source, "sessionHostResult", "wait");
                _hostReleaseCallbacks.Add(() => TriggerClientEvent(source, "sessionHostResult", "free"));
            }

            string hostId;
            // (If no host exists yet null exception is thrown)
            try
            {
                hostId = API.GetHostId();
            }
            catch (NullReferenceException)
            {
                hostId = null;
            }

            if (!String.IsNullOrEmpty(hostId) && API.GetPlayerLastMsg(hostId) < 1000)
            {
                TriggerClientEvent(source, "sessionHostResult", "conflict");
                return;
            }

            _hostReleaseCallbacks.Clear();
            _currentlyHosting = source.Handle;
            Log.Info($"Current game host is '{_currentlyHosting}'");

            TriggerClientEvent(source, "sessionHostResult", "go");

            BaseScript.Delay(5000);
            _currentlyHosting = null;
            _hostReleaseCallbacks.ForEach(f => f());
        }

        private void OnHostedSession([FromSource] Player source)
        {
            if (_currentlyHosting != source.Handle && !String.IsNullOrEmpty(_currentlyHosting))
            {
                Log.Info(
                    $@"Player client ""lying"" about being the host: current host '#{_currentlyHosting}' != client '#{source.Handle}'");
                return;
            }

            _hostReleaseCallbacks.ForEach(f => f());
            _currentlyHosting = null;
            return;
        }
    }
}