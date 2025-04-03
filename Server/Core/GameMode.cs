using System;
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
            }
            else
            {
                var data = user.Character;
                TriggerClientEvent(source, ClientEvents.GameMode.SetupCharacter, data);
            }

            SpawnUser(source, user);
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
            
            SpawnUser(source, user);
        }

        private void SpawnUser([FromSource] CitizenFX.Core.Player player, Domain.User user)
        {
            Log.Info($"Spawn {player.Name}#{user.Id}");
        }
        
    }
}