using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using LostAngeles.Shared;
using LostAngeles.Shared.Weather;
using NLog;

namespace LostAngeles.Server.Core.Weather
{
    public class Syncer : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("WEATHER");
        
        private static string _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Extrasunny);
        private static double _baseTime = 0;
        private static double _timeOffset = 0;
        private static bool _freezeTime = false;
        private static int _newWeatherTimer = 10;
        private static bool _dynamicWeather = true;

        private static readonly Random Rand = new Random();

        public Syncer()
        {
            EventHandlers[ServerEvents.Weather.RequestSyncEvent] += new Action<CitizenFX.Core.Player>(OnRequestSync);
            EventHandlers[ServerEvents.Weather.SetTimeEvent] += new Action<CitizenFX.Core.Player, int, int>(OnSetTime);
            EventHandlers[ServerEvents.Weather.FreezeTimeEvent] += new Action<CitizenFX.Core.Player, int, int>(OnFreezeTime);
            EventHandlers[ServerEvents.Weather.SetWeatherEvent] += new Action<CitizenFX.Core.Player, string>(OnSetWeather);
            EventHandlers[ServerEvents.Weather.FreezeWeatherEvent] += new Action<CitizenFX.Core.Player, int, int>(OnFreezeWeather);

            Tick += UpdateTimeTick;
            Tick += UpdateWeatherTick;
            Tick += AutoUpdateTime;
            Tick += AutoUpdateWeather;
            
            Log.Info($"Weather type: {_currentWeather}");
        }

        private void OnRequestSync([FromSource] CitizenFX.Core.Player source)
        {
            TriggerClientEvent(source, ClientEvents.Weather.UpdateWeatherEvent, _currentWeather);
            TriggerClientEvent(source, ClientEvents.Weather.UpdateTimeEvent, _baseTime, _timeOffset, _freezeTime);
        }

        private void OnSetTime([FromSource] CitizenFX.Core.Player source, int hour, int minute)
        {
            if (AdminHelper.IsAdmin(source) == false) return;
            
            SetTime(hour, minute);
            TriggerClientEvent(source,  ClientEvents.Weather.NotifyEvent, $"Time has changed to {hour:00}:{minute:00}.");
        }

        private void OnFreezeTime([FromSource] CitizenFX.Core.Player source, int hour, int minute)
        {
            if (AdminHelper.IsAdmin(source) == false) return;
            
            _freezeTime = !_freezeTime;
            string message = _freezeTime ? "Time is now frozen." : "Time is no longer frozen.";
            TriggerClientEvent(source, ClientEvents.Weather.NotifyEvent, message);
        }

        private void OnSetWeather([FromSource] CitizenFX.Core.Player source, string weather)
        {
            if (AdminHelper.IsAdmin(source) == false) return;
            
            if (WeatherTypes.IsValidWeather(weather))
            {
                _currentWeather = weather;
                _newWeatherTimer = 10;
                SyncForAll();
                TriggerClientEvent(source, ClientEvents.Weather.NotifyEvent, $"Weather has been changed to {_currentWeather}.");
            }
        }

        private void OnFreezeWeather([FromSource] CitizenFX.Core.Player source, int hour, int minute)
        {
            if (AdminHelper.IsAdmin(source) == false) return;
            
            _dynamicWeather = !_dynamicWeather;
            string status = _dynamicWeather ? "enabled" : "disabled";
            TriggerClientEvent(source, ClientEvents.Weather.NotifyEvent, $"Dynamic weather changes are now ~r~{status}~s~.");
        }

        private void SyncForAll()
        {
            TriggerClientEvent(ClientEvents.Weather.UpdateWeatherEvent, _currentWeather);
            TriggerClientEvent(ClientEvents.Weather.UpdateTimeEvent, _baseTime, _timeOffset, _freezeTime);
        }

        private static async Task UpdateWeatherTick()
        {
            _newWeatherTimer--;
            await Delay(60000);
            if (_newWeatherTimer == 0)
            {
                if (_dynamicWeather)
                {
                    NextWeatherStage();
                }

                _newWeatherTimer = 10;
            }
        }

        private static void NextWeatherStage()
        {
            var currentWeather = WeatherTypes.GetWeatherType(_currentWeather);

            if (currentWeather == WeatherTypes.Types.Clear ||
                currentWeather == WeatherTypes.Types.Clouds ||
                currentWeather == WeatherTypes.Types.Extrasunny)
            {
                var isClearing = Rand.Next(0, 2) == 0;
                if (isClearing)
                {
                    _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Clearing);
                }
                else
                {
                    _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Overcast);
                }
            }
            else if (currentWeather == WeatherTypes.Types.Clearing ||
                     currentWeather == WeatherTypes.Types.Overcast)
            {
                int newWeather = Rand.Next(1, 6);
                switch (newWeather)
                {
                    case 1:
                        if (currentWeather == WeatherTypes.Types.Clearing)
                        {
                            _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Foggy);
                        }
                        else
                        {
                            _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Rain);
                        }

                        break;
                    case 2:
                        _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Clouds);
                        break;
                    case 3:
                        _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Clear);
                        break;
                    case 4:
                        _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Extrasunny);
                        break;
                    case 5:
                        _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Smog);
                        break;
                    default:
                        _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Foggy);
                        break;
                }
            }
            else if (currentWeather == WeatherTypes.Types.Thunder ||
                     currentWeather == WeatherTypes.Types.Rain)
            {
                _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Clearing);
            }
            else if (currentWeather == WeatherTypes.Types.Smog ||
                     currentWeather == WeatherTypes.Types.Foggy)
            {
                _currentWeather = WeatherTypes.GetWeatherString(WeatherTypes.Types.Clearing);
            }

            TriggerClientEvent(ClientEvents.Weather.UpdateWeatherEvent, _currentWeather);
            Log.Info($"New random weather type has been generated: {_currentWeather}");
        }

        private async Task AutoUpdateWeather()
        {
            await Delay(300000);
            TriggerClientEvent(ClientEvents.Weather.UpdateWeatherEvent, _currentWeather);
        }

        private async Task AutoUpdateTime()
        {
            await Delay(5000);
            TriggerClientEvent(ClientEvents.Weather.UpdateTimeEvent, _baseTime, _timeOffset, _freezeTime);
        }

        private async Task UpdateTimeTick()
        {
            var newBaseTime = CalculateBaseTime();
            if (_freezeTime)
            {
                var deltaTime = _baseTime - newBaseTime;
                _timeOffset += deltaTime;
            }

            _baseTime = newBaseTime;
            await Delay(0);
        }

        private double CalculateBaseTime()
        {
            DateTime utcNow = DateTime.UtcNow;
            var unixTimestamp = new DateTimeOffset(utcNow).ToUnixTimeSeconds();
            var result = (unixTimestamp / 2) + 360;
            return result;
        }

        public static string GetTimeString()
        {
            var hour = (int)((_baseTime + _timeOffset) / 60) % 24;
            var minute = (int)((_baseTime + _timeOffset) % 60);
            return $"{hour:00}:{minute:00}";
        }

        private static void SetTime(int hour, int minute)
        {
            hour = Shared.Math.Clamp(hour, 0, 24);
            minute = Shared.Math.Clamp(minute, 0, 60);
            ShiftToHour(hour);
            ShiftToMinute(minute);
            TriggerClientEvent(ClientEvents.Weather.UpdateTimeEvent, _baseTime, _timeOffset, _freezeTime);
        }

        private static void ShiftToHour(int hour)
        {
            _timeOffset -= ((_baseTime + _timeOffset) / 60 % 24 - hour) * 60;
        }

        private static void ShiftToMinute(int minute)
        {
            _timeOffset -= ((_baseTime + _timeOffset) % 60 - minute);
        }
    }
}