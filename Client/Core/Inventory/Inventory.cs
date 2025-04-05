using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core.Inventory
{
    public partial class Inventory : BaseScript
    {

        public Inventory()
        {
            EventHandlers[ClientEvents.Inventory.OpenInventoryEvent] += new Action(OnOpenInventory);

            RegisterNuiCallbacks();
        }

        private void OnOpenInventory()
        {
            if (API.IsPauseMenuActive() || API.IsNuiFocused())
            {
                return;
            }

            Debug.WriteLine("Opening inventory");
            ToggleInventoryVisible(true);
        }
    }
}