using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core
{
    public class HardCap : BaseScript
    {
        public HardCap()
        {
            Tick += PlayerActivatedCheck;
        }

        private async Task PlayerActivatedCheck()
        {
            if (API.NetworkIsSessionStarted())
            {
                TriggerServerEvent(ServerEvents.HardCap.PlayerActivatedEvent);
                Tick -= PlayerActivatedCheck;
            }
            await Task.FromResult(0);
        }
    }
}