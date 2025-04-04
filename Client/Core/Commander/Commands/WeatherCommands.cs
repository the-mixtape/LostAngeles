using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace LostAngeles.Client.Core.Commander.Commands
{
    public class WeatherCommands
    {
        public async Task OnShowTime(int source, List<object> args, string rawCommand)
        {
            var timeString = Weather.Syncer.GetTimeString();
            Debug.WriteLine($"time: {timeString}");
            await Task.FromResult(0);
        }
        
        public async Task OnShowWeather(int source, List<object> args, string rawCommand)
        {
            var weather = Weather.Syncer.GetWeatherString();
            Debug.WriteLine($"Weather: {weather}");
            await Task.FromResult(0);
        }
        
        public async Task OnSetTime(int source, List<object> args, string rawCommand)
        {
            if (args.Count != 2)
            {
                Debug.WriteLine($"settime [hour] [minute]");
                return;
            }
                
            if (!int.TryParse(args[0].ToString(), out var hour))
            {
                Debug.WriteLine($"First argument is invalid");
                return;
            }
                    
            if (!int.TryParse(args[1].ToString(), out var minute))
            {
                Debug.WriteLine($"Second argument is invalid");
                return;
            }
                    
            hour = Shared.Math.Clamp(hour, 0, 24);
            minute = Shared.Math.Clamp(minute, 0, 60);
            Weather.Syncer.SetTime(hour, minute);
            await Task.FromResult(0);
        }
        
        public async Task OnFreezeTime(int source, List<object> args, string rawCommand)
        {
            Weather.Syncer.ToggleFreezeTime();
            await Task.FromResult(0);
        }
        
        public async Task OnSetWeather(int source, List<object> args, string rawCommand)
        {
            if (args.Count != 1)
            {
                Debug.WriteLine($"setweather [weathertype]");
                return;
            }

            string weather = args[0].ToString();
            if (!Shared.Weather.WeatherTypes.IsValidWeather(weather))
            {
                string avaiableWeather = Shared.Weather.WeatherTypes.GetAvailableWeatherTypes();
                // UI.ShowNotification($"Invalid weather type, valid weather types are: {avaiableWeather}");
                return;
            }
            
            Weather.Syncer.SetWeather(weather);
            await Task.FromResult(0);
        }
        
        public async Task OnFreezeWeather(int source, List<object> args, string rawCommand)
        {
            Weather.Syncer.ToggleFreezeWeather();
            await Task.FromResult(0);
        }
    }
}