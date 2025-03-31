using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace LostAngeles.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Debug.WriteLine("Hi from LostAngeles.Client!");
        }

        [Tick]
        public Task OnTick()
        {

            return Task.FromResult(0);
        }
    }
}