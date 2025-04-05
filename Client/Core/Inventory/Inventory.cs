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
            EventHandlers[ClientEvents.Inventory.ShowQuickSlotsEvent] += new Action(OnShowQuickSlots);
            EventHandlers[ClientEvents.Inventory.HideQuickSlotsEvent] += new Action(OnHideQuickSlots);

            RegisterNuiCallbacks();
        }

        private void OnOpenInventory()
        {
            if (API.IsPauseMenuActive()) // API.IsNuiFocused()
            {
                return;
            }
            
            ToggleInventoryVisible(true);
        }

        private void OnShowQuickSlots()
        {
            ToggleQuickSlotsVisible(true);
        }
        
        private void OnHideQuickSlots()
        {
            ToggleQuickSlotsVisible(false);
        }
    }
}