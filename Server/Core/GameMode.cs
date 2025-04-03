using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Server.Repository;
using LostAngeles.Shared;
using NLog;

namespace LostAngeles.Server.Core
{
    public class GameMode : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("GAMEMODE");
        public static IUser UserRepo { get; set; }
        
        public GameMode()
        {
            EventHandlers[ServerEvents.GameMode.InitializeCharacterEvent] += new Action<CitizenFX.Core.Player>(OnInitializeCharacter);
            EventHandlers[ServerEvents.GameMode.FinishedCustomizeCharacter] += new Action<CitizenFX.Core.Player, string>(OnFinishedCustomizeCharacter);
        }

        private async void OnInitializeCharacter([FromSource] CitizenFX.Core.Player source)
        {
            var licenseIdentifier = Helper.GetLicense(source);
            if (string.IsNullOrEmpty(licenseIdentifier))
            {
                API.DropPlayer(source.Handle, "Error receiving license identifier.");
                return;
            }
         
            var user = await UserRepo.GetOrCreate(licenseIdentifier);
            if (user == null)
            {
                API.DropPlayer(source.Handle, "Couldn't get user information, try again later.");
                return;
            }
            
            if (string.IsNullOrEmpty(user.Character))
            {
                TriggerClientEvent(source, ClientEvents.GameMode.CustomizeCharacter);
                return;
            }
            
            var data = user.Character;
            TriggerClientEvent(source, ClientEvents.GameMode.SetupCharacter, data);

            await SpawnUser(source, user);
        }


        private async void OnFinishedCustomizeCharacter([FromSource] CitizenFX.Core.Player source, string data)
        {
            var licenseIdentifier = Helper.GetLicense(source);
            if (string.IsNullOrEmpty(licenseIdentifier))
            {
                API.DropPlayer(source.Handle, "Error receiving license identifier");
                return;
            }
            
            bool success = await UserRepo.UpdateCharacter(licenseIdentifier, data);
            if (!success)
            {   
                API.DropPlayer(source.Handle, "Couldn't save the character, please try again later.");
                return;
            }
            Log.Info($"Successfully updated the character ({source.Name}|{licenseIdentifier}).");
            
            var user = await UserRepo.GetOrCreate(licenseIdentifier);
            if (user == null)
            {
                API.DropPlayer(source.Handle, "Couldn't get user information, try again later.");
                return;
            }
            
            await SpawnUser(source, user);
        }

        private async Task SpawnUser([FromSource] CitizenFX.Core.Player player, Domain.User user)
        {
            SpawnPosition spawnPosition = user.Position?.ToSpawnPosition();
            if (user.Position == null)
            {
                spawnPosition = SpawnHelper.GetNextSpawnPosition();
                var position = spawnPosition.ToPosition();
                var success = await UserRepo.UpdatePosition(user.License, position);
                if (!success)
                {   
                    API.DropPlayer(player.Handle, "Couldn't save the position, please try again later.");
                    return;
                }
            }

            if (spawnPosition == null)
            {
                Log.Error("Spawn position is null.");
                API.DropPlayer(player.Handle, "Couldn't get spawn position, please try again later.");
                return;
            }
            
            var data = Converter.ToJson(spawnPosition);
            TriggerClientEvent(player, ClientEvents.GameMode.SpawnPlayer, data);

            Log.Debug($"Spawning {player.Name}#{user.Id} at {spawnPosition}");
        }
    }
}