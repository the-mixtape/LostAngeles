namespace LostAngeles.Shared
{
    public abstract class ClientEvents
    {
        public abstract class Client
        {
            public const string SetupClientConfigEvent = "Client::SetupClientConfig";
        }

        public abstract class GameMode
        {
            public const string InitializeEvent = "GameMode::Initialize";
            public const string CustomizeCharacter = "GameMode::CustomizeCharacter";
        }
    }   
}