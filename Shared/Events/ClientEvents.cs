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
            public const string SetupCharacter = "GameMode::SetupCharacter";
            public const string SpawnPlayer = "GameMode::SpawnPlayer";
        }

        public abstract class CharacterCustomization
        {
            public const string StartCustomizeEvent = "CharacterCustomization::StartCustomize";
            public const string EndCustomizeEvent = "CharacterCustomization::EndCustomize";
            public const string RefreshModelEvent = "CharacterCustomization::RefreshModel";
            public const string OnFinishedCallback = "CharacterCustomization::OnFinishedCallback";
        }
    }   
}