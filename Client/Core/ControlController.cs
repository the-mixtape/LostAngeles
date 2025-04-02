using CitizenFX.Core.Native;

namespace LostAngeles.Client.Core
{
    public static class ControlController
    {
        public static bool HasControl { get; private set; }

        public static void TogglePlayerControl(bool value)
        {
            HasControl = value;
            API.SetPlayerControl(API.PlayerId(), value, 0);
        }
    }
}