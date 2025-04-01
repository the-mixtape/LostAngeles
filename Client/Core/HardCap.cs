using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace LostAngeles.Client.Core
{
    public class HardCap : BaseScript
    {
        public HardCap()
        {
            Tick += PlayerActivatedCheck;
        }

        async Task PlayerActivatedCheck()
        {
            if (API.NetworkIsSessionStarted())
            {
                TriggerServerEvent("HardCap::PlayerActivated");
                Tick -= PlayerActivatedCheck;
            }
            await Task.FromResult(0);
        }
    }
}