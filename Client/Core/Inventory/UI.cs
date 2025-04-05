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

        private class InventoryMessage : NuiMessage
        {
            public bool Display { get; set; }

            public InventoryMessage()
            {
                Type = "Inventory";
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

            // var displayString = display.ToString().ToLower();
            // string json = "{" +
            // "\"Type\": \"Inventory\"," +
            // $"\"Display\": {displayString}" +
            // "}";

            var msg = new InventoryMessage()
            {
                Display = display,
            };

            var json = Converter.ToJson(msg);
            API.SendNuiMessage(json);
            API.SetNuiFocus(display, display);
        }

        private static string GetNuiHandlerName(string eventName)
        {
            return $"__cfx_nui:{eventName}";
        }
    }
}