using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core.Inventory
{
    public partial class Inventory
    {
        private class NuiMessage
        {
            public string Type { get; set; }
        }

        private class DisplayInventoryMessage : NuiMessage
        {
            public bool Display { get; set; }

            public DisplayInventoryMessage()
            {
                Type = "showInventory";
            }
        }

        private class DisplayQuickSlotsMessage : NuiMessage
        {
            public bool Display { get; set; }

            public DisplayQuickSlotsMessage()
            {
                Type = "showQuickSlots";
            }
        }

        private const string HudPostFx = "SwitchHUDIn";
        private bool Displayed { get; set; } = false;

        private void RegisterNuiCallbacks()
        {
            API.RegisterNuiCallbackType(ClientEvents.Inventory.CloseInventoryNuiCallback);
            EventHandlers[GetNuiHandlerName(ClientEvents.Inventory.CloseInventoryNuiCallback)] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
                {
                    ToggleInventoryVisible(false);
                });
        }

        private void PlayHudPostFx(bool play)
        {
            if (play)
            {
                API.AnimpostfxPlay(HudPostFx, 0, true);
            }
            else
            {
                API.AnimpostfxStop(HudPostFx);
            }
        }

        private void ToggleInventoryVisible(bool display)
        {
            if (Displayed == display) return;
            Displayed = display;

            PlayHudPostFx(display);

            var msg = new DisplayInventoryMessage()
            {
                Display = display,
            };

            var json = Converter.ToJson(msg);
            API.SendNuiMessage(json);
            API.SetNuiFocus(display, display);
        }

        private void ToggleQuickSlotsVisible(bool display)
        {
            var msg = new DisplayQuickSlotsMessage()
            {
                Display = display,
            };
            
            var json = Converter.ToJson(msg);
            API.SendNuiMessage(json);
        }

        private static string GetNuiHandlerName(string eventName)
        {
            return $"__cfx_nui:{eventName}";
        }
    }
}