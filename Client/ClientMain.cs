using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Client.Core.Player;
using LostAngeles.Shared;
using LostAngeles.Shared.Config;

namespace LostAngeles.Client
{
    public class ClientMain : BaseScript
    {
        private static ClientConfig _config;
        
        public ClientMain()
        {
            Tick += SessionStartedCheck;

            EventHandlers[ClientEvents.Client.SetupClientConfigEvent] += new Action<string>(OnSetupClientConfig);
        }

        private async Task SessionStartedCheck()
        {
            if (API.NetworkIsSessionStarted())
            {
                Tick -= SessionStartedCheck;
                OnSessionStarted();
            }

            await Task.FromResult(0);
        }

        private void OnSessionStarted()
        {
            TriggerServerEvent(ServerEvents.Server.RequestClientConfigEvent);
        }

        private void OnSetupClientConfig(string data)
        {
            _config = Converter.FromJson<ClientConfig>(data);
            CrouchCrawl.Initialize(_config.CanCrouch, _config.CanCrawl);
            PositionUpdater.Initialize(_config.PositionUpdateDelay);
            
            TriggerEvent(ClientEvents.GameMode.InitializeEvent);
        }
    }
}