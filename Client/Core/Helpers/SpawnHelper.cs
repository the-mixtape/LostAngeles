using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core
{
    public class SpawnHelper : BaseScript
    {
        private const int FadeOutTime = 500;
        private const int FadeInTime = 500;
        private const int CollisionLoadDelay = 5000;

        private static bool _spawnLock;

        public static void Spawn(SpawnPosition position, Action callback = null)
        {
            _ = AsyncSpawn(position, callback);
        }

        private static async Task AsyncSpawn(SpawnPosition position, Action callback)
        {
            if (_spawnLock)
            {
                return;
            }

            _spawnLock = true;

            API.DoScreenFadeOut(FadeOutTime);
            while (!API.IsScreenFadedOut())
            {
                await Delay(0);
            }

            var playerId = API.PlayerId();
            FreezePlayer(playerId, true);

            // if (Model != null)
            // {
            //     var modelHash = (uint)Model;
            //     while (!API.HasModelLoaded(modelHash))
            //     {
            //         API.RequestModel(modelHash);
            //         await Delay(0);
            //     }
            //
            //     API.SetPlayerModel(playerId, modelHash);
            //     API.SetModelAsNoLongerNeeded(modelHash);
            // }

            API.RequestCollisionAtCoord(position.Location.X, position.Location.Y, position.Location.Z);

            var ped = API.PlayerPedId();
            API.SetEntityCoordsNoOffset(ped,
                position.Location.X, position.Location.Y, position.Location.Z,
                false, false, false);

            API.NetworkResurrectLocalPlayer(position.Location.X, position.Location.Y, position.Location.Z,
                position.Heading, true, true);

            API.ClearPedTasksImmediately(ped);
            API.RemoveAllPedWeapons(ped, true);
            API.ClearPlayerWantedLevel(playerId);

            var time = API.GetGameTimer();
            while (!API.HasCollisionLoadedAroundEntity(ped) && API.GetGameTimer() - time < CollisionLoadDelay)
            {
                await Delay(0);
            }

            API.ShutdownLoadingScreen();

            if (API.IsScreenFadedOut())
            {
                API.DoScreenFadeIn(FadeInTime);
                while (!API.IsScreenFadedIn())
                {
                    await Delay(0);
                }
            }

            FreezePlayer(playerId, false);

            _spawnLock = false;
            
            callback?.Invoke();
        }


        private static void FreezePlayer(int playerId, bool freeze)
        {
            API.SetPlayerControl(playerId, !freeze, 0);
            var ped = API.GetPlayerPed(playerId);

            if (!freeze)
            {
                if (!API.IsEntityVisible(ped))
                {
                    API.SetEntityVisible(ped, true, false);
                }

                if (!API.IsPedInAnyVehicle(ped, true))
                {
                    API.SetEntityCollision(ped, true, false);
                }

                API.FreezeEntityPosition(ped, false);
                API.SetPlayerInvincible(playerId, false);
            }
            else
            {
                if (!API.IsEntityVisible(ped))
                {
                    API.SetEntityVisible(ped, false, false);
                }

                API.SetEntityCollision(ped, false, false);
                API.FreezeEntityPosition(ped, true);
                API.SetPlayerInvincible(playerId, true);

                if (!API.IsPedFatallyInjured(ped))
                {
                    API.ClearPedTasksImmediately(ped);
                }
            }
        }
    }
}