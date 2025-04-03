using System;
using CitizenFX.Core;
using LostAngeles.Server.Repository;
using LostAngeles.Shared;
using NLog;

namespace LostAngeles.Server.Core.Player
{
    public class PositionUpdater : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("POSITIONUPDATER");
        public static IUser UserRepo { get; set; }
        
        public PositionUpdater()
        {
            EventHandlers[ServerEvents.PositionUpdater.UpdatePositionEvent] += new Action<CitizenFX.Core.Player, string>(OnUpdatePosition);
        }

        private async void OnUpdatePosition([FromSource] CitizenFX.Core.Player source, string data)
        {
            if (UserRepo == null)
            {
                Log.Error("User repo is null");
                return;
            }
            
            var playerPosition = Converter.FromJson<PlayerPosition>(data);
            
            var licenseIdentifier = Helper.GetLicense(source);
            var position = playerPosition.ToPosition();
            
            var success = await UserRepo.UpdatePosition(licenseIdentifier, position);
            if (!success)
            {
                Log.Warn("Failed to update position");
            }
        }
    }
}