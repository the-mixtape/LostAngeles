using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;
using LostAngeles.Shared.Weather;

namespace LostAngeles.Client.Core.Weather
{
    public class Syncer : BaseScript
    {
        private const float WeatherTransitionTime = 15.0f;
        
        private static string _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Extrasunny);
        private static  string _lastWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Extrasunny);
        
        private static double _baseTime = 0;
        private static double _timeOffset = 0;
        private static int _timer = 0;
        private static bool _freezeTime;

        public Syncer()
        {
            EventHandlers[ClientEvents.Weather.UpdateWeatherEvent] += new Action<string>(OnUpdateWeather);
            EventHandlers[ClientEvents.Weather.UpdateTimeEvent] += new Action<double, double, bool>(OnUpdateTime);
            EventHandlers[ClientEvents.Weather.NotifyEvent] += new Action<string>(OnNotify);
            
            Tick += UpdateWeatherTick;
            Tick += UpdateTimeTick;
            
            RequestSync();
        }

        private static void RequestSync()
        {
            TriggerServerEvent(ServerEvents.Weather.RequestSyncEvent);
        }

        private void OnUpdateWeather(string newWeather)
        {
            _currentWeather = newWeather;
        }

        private void OnUpdateTime(double baseTime, double offset, bool freeze)
        {
            _freezeTime = freeze;
            _timeOffset = offset;
            _baseTime = baseTime;
        }

        private async Task UpdateWeatherTick()
        {
            if (_lastWeather != _currentWeather)
            {
                _lastWeather = _currentWeather;
                API.SetWeatherTypeOverTime(_currentWeather, WeatherTransitionTime);
                await Delay((int)WeatherTransitionTime * 1000);
            }

            // SetBlackout(_blackout);
            // ClearOverrideWeather();
            // ClearWeatherTypePersist();
            API.SetWeatherTypePersist(_lastWeather);
            API.SetWeatherTypeNow(_lastWeather);
            API.SetWeatherTypeNowPersist(_lastWeather);

            var isXmas = WeatherTypes.GetWeatherType(_lastWeather) == WeatherTypes.Types.Xmas;
            API.SetForceVehicleTrails(isXmas);
            API.SetForcePedFootstepsTracks(isXmas);
            await Delay(100);
        }


        private async Task UpdateTimeTick()
        {
            var newBaseTime = _baseTime;
            if (API.GetGameTimer() - 500 > _timer)
            {
                newBaseTime += 0.25f;
                _timer = API.GetGameTimer();
            }

            if (_freezeTime)
            {
                var deltaTime = _baseTime - newBaseTime;
                _timeOffset += deltaTime;
            }

            _baseTime = newBaseTime;
            var hour = (int)((_baseTime + _timeOffset) / 60) % 24;
            var minute = (int)((_baseTime + _timeOffset) % 60);
            API.NetworkOverrideClockTime(hour, minute, 0);

            await Delay(0);
        }
        
        public static string GetTimeString()
        {
            var hour = (int)((_baseTime + _timeOffset) / 60) % 24;
            var minute = (int)((_baseTime + _timeOffset) % 60);
            return $"{hour:00}:{minute:00}";
        }

        public static string GetWeatherString()
        {
            return _currentWeather;
        }
        
        public static void SetTime(int hour, int minute)
        {
            hour = Shared.Math.Clamp(hour, 0, 24);
            minute = Shared.Math.Clamp(minute, 0, 60);
            TriggerServerEvent(ServerEvents.Weather.SetTimeEvent, hour, minute);
        }


        public static void SetWeather(string weather)
        {
            if(WeatherTypes.IsValidWeather(weather))
            {
                TriggerServerEvent(ServerEvents.Weather.SetWeatherEvent, weather);  
            };
        }

        public static void ToggleFreezeTime()
        {
            TriggerServerEvent(ServerEvents.Weather.FreezeTimeEvent);
        }
        
        public static void ToggleFreezeWeather()
        {
            TriggerServerEvent(ServerEvents.Weather.FreezeWeatherEvent);
        }


        private void OnNotify(string message)
        {
            // UI.ShowNotification(message);
        }
        
    }
}