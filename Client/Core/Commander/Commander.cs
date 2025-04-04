using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Client.Core.Commander.Commands;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core.Commander
{
    public class Commander : BaseScript
    {
        private readonly WeatherCommands _weatherCommands = new WeatherCommands();

        public Commander()
        {
            EventHandlers[ClientEvents.Commander.InitializeEvent] += new Action(OnInitialize);
            EventHandlers[ClientEvents.Commander.SetupClientEvent] += new Action<bool>(OnSetupClient);
        }

        private void OnInitialize()
        {
            TriggerServerEvent(ServerEvents.Commander.InitializeClientEvent);
        }

        private void OnSetupClient(bool isAdmin)
        {
            RegisterBaseCommand("time", "Show the time.", _weatherCommands.OnShowTime);
            RegisterBaseCommand("weather", "Show the weather.", _weatherCommands.OnShowWeather);

            if (!isAdmin) return;

            RegisterBaseCommand("settime", "Set the time.", _weatherCommands.OnSetTime);
            RegisterBaseCommand("freezetime", "Stop time.", _weatherCommands.OnFreezeTime);

            var weatherTypes = Shared.Weather.WeatherTypes.GetAvailableWeatherTypes();
            RegisterBaseCommand("setweather", $"Set weather: {weatherTypes}.", _weatherCommands.OnSetWeather);
            RegisterBaseCommand("freezeweather", "Stop weather.", _weatherCommands.OnFreezeWeather);
        }

        private void RegisterBaseCommand(string commandName, string commandChatSuggestion,
            Func<int, List<object>, string, Task> command)
        {
            Debug.WriteLine($"Register command: {commandName}");
            API.RegisterCommand(commandName, new Func<int, List<object>, string, Task>(command), false);
            TriggerEvent("chat:addSuggestion", $"/{commandName}", commandChatSuggestion);
        }
    }
}