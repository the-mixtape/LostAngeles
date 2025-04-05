using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core.Player
{
    public class InputController : BaseScript
    {
        private const string OpenInventoryCommand = "inventory";
        private const string OpenInventoryKeyBind = "TAB";
        private const string OpenInventoryKeyBindDescription = "Open Inventory";

        public InputController()
        {
            EventHandlers[ClientEvents.Player.InitializeInputControllerEvent] +=
                new Action(OnInitializeInputController);
        }

        private void OnInitializeInputController()
        {
            RegisterInventoryInputs();
        }

        private void RegisterInventoryInputs()
        {
            API.RegisterKeyMapping(OpenInventoryCommand, OpenInventoryKeyBindDescription, "keyboard",
                OpenInventoryKeyBind);
            API.RegisterCommand(OpenInventoryCommand, new Func<Task>(() =>
            {
                OnOpenInventoryCommand();
                return Task.FromResult(true);
            }), false);
        }

        private void OnOpenInventoryCommand()
        {
            if (Game.Player.IsDead) return;
            TriggerEvent(ClientEvents.Inventory.OpenInventoryEvent);
        }
    }
}