using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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

        public GameMode()
        {
            EventHandlers[ClientEvents.GameMode.InitializeEvent] += new Action(OnStartInitialize);
            EventHandlers[ClientEvents.GameMode.CustomizeCharacter] += new Action(OnCustomizeCharacter);
        }

        private void OnStartInitialize()
        {
            Status = GameStatus.Initializing;
        }

        private void OnCustomizeCharacter()
        {
            Status = GameStatus.CharacterCustomization;
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
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Uniq);
            TriggerServerEvent(ServerEvents.GameMode.InitializeCharacterEvent);
        }

        private void UpdateCharacterCustomizationStatus()
        {
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Uniq);
            Debug.WriteLine("Go to Customization Character");
        }

        private void UpdateGameplayStatus()
        {
            BucketController.SetLocalPLayerRoutingBucket(RoutingBucketTypes.Default);
        }
    }
}