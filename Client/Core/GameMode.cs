using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Client.Core.Data;
using LostAngeles.Shared;
using Mono.CSharp;

namespace LostAngeles.Client.Core
{
    public class GameMode : BaseScript
    {
        private enum GameStatus
        {
            None = 0,
            Initializing = 1,
            CharacterCustomization = 2,
            Spawning = 3,
            Gameplay = 4,
        }

        private GameStatus _status = GameStatus.None;

        private GameStatus GetStatus()
        {
            return _status;
        }

        private async Task SetStatus(GameStatus status)
        {
            _status = status;
            await Update();
        }

        private readonly SpawnPosition
            _characterEditPosition = new SpawnPosition(402.89f, -996.87f, -99.0f, 180);

        private const int SpawnFadeOutInTime = 1500;

        public GameMode()
        {
            EventHandlers[ClientEvents.GameMode.InitializeEvent] += new Action(OnStartInitialize);
            EventHandlers[ClientEvents.GameMode.CustomizeCharacter] += new Action(OnCustomizeCharacter);
            EventHandlers[ClientEvents.GameMode.SpawnPlayer] += new Action<string>(OnSpawnPlayer);

            EventHandlers[ClientEvents.GameMode.SetupCharacter] += new Action<string>(OnSetupCharacter);

            EventHandlers[ClientEvents.CharacterCustomization.OnFinishedCallback] += new Action(OnCustomizerFinished);
        }

        private async void OnStartInitialize()
        {
            await SetStatus(GameStatus.Initializing);
        }

        private async void OnCustomizeCharacter()
        {
            await SetStatus(GameStatus.CharacterCustomization);
        }

        private async void OnSpawnPlayer(string data)
        {
            await SetStatus(GameStatus.Spawning);

            var position = Converter.FromJson<SpawnPosition>(data);

            await SpawnHelper.SpawnAsync(position);
            await SetStatus(GameStatus.Gameplay);
        }

        private void OnSetupCharacter(string data)
        {
            ClientData.Character = Converter.FromJson<Character>(data);
        }

        private void OnCustomizerFinished()
        {
            var data = Converter.ToJson(ClientData.Character);
            TriggerServerEvent(ServerEvents.GameMode.FinishedCustomizeCharacter, data);
        }

        private async Task Update()
        {
            switch (GetStatus())
            {
                case GameStatus.None:
                    await UpdateNoneStatus();
                    break;
                case GameStatus.Initializing:
                    await UpdateInitializingStatus();
                    break;
                case GameStatus.CharacterCustomization:
                    await UpdateCharacterCustomizationStatus();
                    break;
                case GameStatus.Spawning:
                    await UpdateSpawningStatus();
                    break;
                case GameStatus.Gameplay:
                    await UpdateGameplayStatus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task UpdateNoneStatus()
        {
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Default);
            await Task.FromResult(0);
        }

        private async Task UpdateInitializingStatus()
        {
            ControlController.TogglePlayerControl(false);
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Uniq);
            TriggerServerEvent(ServerEvents.GameMode.InitializeCharacterEvent);
            await Task.FromResult(0);
        }

        private async Task UpdateCharacterCustomizationStatus()
        {
            ControlController.TogglePlayerControl(false);
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Uniq);

            // TODO: hide game ui
            // API.DisplayRadar(false);
            
            await SpawnHelper.SpawnAsync(_characterEditPosition);
            TriggerEvent(ClientEvents.CharacterCustomization.StartCustomizeEvent);
        }

        private async Task UpdateSpawningStatus()
        {
            API.DoScreenFadeOut(SpawnFadeOutInTime);
            await CharacterCustomizer.CharacterCustomizer.RefreshModel();
        }

        private async Task UpdateGameplayStatus()
        {
            API.DoScreenFadeIn(SpawnFadeOutInTime);
            while (!API.IsScreenFadedIn())
            {
                await Delay(0);
            }
            
            ControlController.TogglePlayerControl(true);
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Default);
            
            await Task.FromResult(0);
        }
    }
}