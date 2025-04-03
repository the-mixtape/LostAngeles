using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Client.Core.Data;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core
{
    public class GameMode : BaseScript
    {
        private enum GameStatus
        {
            None = 0,
            Initializing = 1,
            CharacterCustomization = 2,
            Gameplay = 3,
        }

        private GameStatus _status = GameStatus.None;

        private GameStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                Update();
            }
        }

        private readonly SpawnPosition _characterEditPosition = new SpawnPosition(402.89f, -996.87f, -99.0f, 180); //173.97f

        public GameMode()
        {
            EventHandlers[ClientEvents.GameMode.InitializeEvent] += new Action(OnStartInitialize);
            EventHandlers[ClientEvents.GameMode.CustomizeCharacter] += new Action(OnCustomizeCharacter);
            EventHandlers[ClientEvents.GameMode.SetupCharacter] += new Action<string>(OnSetupCharacter);
            
            EventHandlers[ClientEvents.CharacterCustomization.OnFinishedCallback] += new Action(OnCustomizerFinished);
        }

        private void OnStartInitialize()
        {
            Status = GameStatus.Initializing;
        }

        private void OnCustomizeCharacter()
        {
            Status = GameStatus.CharacterCustomization;
        }

        private void OnSetupCharacter(string data)
        {
            ClientData.Character = Converter.FromJson<Character>(data);
            TriggerEvent(ClientEvents.CharacterCustomization.RefreshModelEvent);
        }

        private void OnCustomizerFinished()
        {
            var data = Converter.ToJson(ClientData.Character);
            TriggerServerEvent(ServerEvents.GameMode.FinishedCustomizeCharacter, data);
        }

        private void Update()
        {
            switch (Status)
            {
                case GameStatus.None:
                    UpdateNoneStatus();
                    break;
                case GameStatus.Initializing:
                    UpdateInitializingStatus();
                    break;
                case GameStatus.CharacterCustomization:
                    UpdateCharacterCustomizationStatus();
                    break;
                case GameStatus.Gameplay:
                    UpdateGameplayStatus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateNoneStatus()
        {
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Default);
        }

        private void UpdateInitializingStatus()
        {
            ControlController.TogglePlayerControl(false);
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Uniq);
            TriggerServerEvent(ServerEvents.GameMode.InitializeCharacterEvent);
        }

        private void UpdateCharacterCustomizationStatus()
        {
            ControlController.TogglePlayerControl(false);
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Uniq);

            // TODO: hide game ui
            // API.DisplayRadar(false);

            SpawnHelper.Spawn(_characterEditPosition,
                () =>
                {
                    ControlController.TogglePlayerControl(false);
                    TriggerEvent(ClientEvents.CharacterCustomization.StartCustomizeEvent);
                });
        }

        private void UpdateGameplayStatus()
        {
            ControlController.TogglePlayerControl(true);
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Default);
        }
    }
}