using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;
using Mono.CSharp;

namespace LostAngeles.Client.Core.Player
{
    public class PositionUpdater : BaseScript
    {
        private const int TickDelay = 10 * 1000; //TODO: to config

        private static bool _enabled = false;
        private PlayerPosition _lastPosition;

        public PositionUpdater()
        {
            Tick += UpdatePositionTick;
        }

        public static void Enable()
        {
            _enabled = true;
        }

        public static void Disable()
        {
            _enabled = false;
        }

        private async Task UpdatePositionTick()
        {
            if (!_enabled) return;

            var currentPos = GetCurrentPlayerPosition();
            if (!currentPos.Compare(_lastPosition, 5.0f))
            {
                var data = Converter.ToJson(currentPos);
                TriggerServerEvent(ServerEvents.PositionUpdater.UpdatePositionEvent, data);
            }

            _lastPosition = currentPos;
            await Delay(TickDelay);
        }

        private static PlayerPosition GetCurrentPlayerPosition()
        {
            var playerPed = API.PlayerPedId();
            var position = API.GetEntityCoords(playerPed, true);
            var heading = API.GetEntityHeading(playerPed);

            return new PlayerPosition
            {
                Location = new Vector3(position.X, position.Y, position.Z),
                Heading = heading,
            };
        }
    }
}