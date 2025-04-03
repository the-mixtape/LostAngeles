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
    }
}