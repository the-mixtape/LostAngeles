namespace LostAngeles.Shared
{
    public abstract class ServerEvents
    {
        public abstract class BucketController
        {
            public const string SetBucketEvent = "BucketController::OnSetBucket";
        }

        public abstract class Server
        {
            public const string RequestClientConfigEvent = "Server::RequestClientConfig";
        }

        public abstract class HardCap
        {
            public const string PlayerActivatedEvent = "HardCap::PlayerActivated";
        }

        public abstract class GameMode
        {
            public const string InitializeCharacterEvent = "GameMode::InitializeCharacter";
            public const string FinishedCustomizeCharacter = "GameMode::FinishedCustomizeCharacter";
        }

        public abstract class PositionUpdater
        {
            public const string UpdatePositionEvent = "PositionUpdater::UpdatePosition";
        }

        public abstract class Weather
        {
            public const string RequestSyncEvent = "Weather::RequestSync";
            public const string SetTimeEvent = "Weather::SetTime";
            public const string SetWeatherEvent = "Weather::SetWeather";
            public const string FreezeTimeEvent = "Weather::FreezeTime";
            public const string FreezeWeatherEvent = "Weather::FreezeWeather";
        }

        public abstract class Commander
        {
            public const string InitializeClientEvent = "Commander::InitializeClient";
        }
    }
}