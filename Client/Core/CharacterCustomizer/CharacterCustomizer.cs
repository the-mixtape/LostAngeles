using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Client.Core.Data;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core.CharacterCustomizer
{
    public partial class CharacterCustomizer : BaseScript
    {
        private readonly CustomizerSettings _settings = new CustomizerSettings();

        private const string MaleGender = "Male";
        private const string FemaleGender = "Female";

        private readonly List<dynamic> _genders = new List<dynamic>()
        {
            MaleGender,
            FemaleGender
        };

        #region RunTime

        private int _bodyCamera, _faceCamera, _enterCamera;

        #endregion

        public CharacterCustomizer()
        {
            EventHandlers[ClientEvents.CharacterCustomization.StartCustomizeEvent] += new Action(OnStartCustomize);
            EventHandlers[ClientEvents.CharacterCustomization.EndCustomizeEvent] += new Action(OnEndCustomize);
            EventHandlers[ClientEvents.CharacterCustomization.RefreshModelEvent] += new Action(OnRefreshModel);
        }
        
        public static async Task RefreshModel()
        {
            var playerPed = API.PlayerPedId();

            var modelHash = GetModelHash();
            if ((uint)API.GetEntityModel(playerPed) == modelHash) return;

            while (!API.HasModelLoaded(modelHash))
            {
                API.RequestModel(modelHash);
                await Delay(0);
            }

            API.SetPlayerModel(API.PlayerId(), modelHash);
            ChangeComponents();
        }


        private async void OnStartCustomize()
        {
            API.DoScreenFadeOut(0);
            while (!API.IsScreenFadedOut())
            {
                await Delay(0);
            }

            await RefreshModel();

            CreateCameras();
            await PlayEnterCameraAnimation();

            CreateMainMenu();
        }

        private async void OnEndCustomize()
        {
            DeleteMainMenu();
            
            API.DoScreenFadeOut(CustomizerSettings.ExitAnimFadeOutTime);
            while (!API.IsScreenFadedOut())
            {
                await Delay(0);
            }   
            
            API.RenderScriptCams(false, false, 0, true, true);
            DeleteCameras();
            
            TriggerEvent(ClientEvents.CharacterCustomization.OnFinishedCallback);
        }

        private async void OnRefreshModel()
        {
            await RefreshModel();
        }

        private void CreateCameras()
        {
            // creating cameras relative to the character
            var playerPed = API.PlayerPedId();
            var position = API.GetEntityCoords(playerPed, true);
            var forwardVector = API.GetEntityForwardVector(playerPed);
            Vector3 cameraPosition = position + forwardVector * _settings.BodyCameraDistance;

            _settings.BodyCamera.Position = cameraPosition;
            _settings.EnterCamera.Position = cameraPosition;
            _settings.FaceCamera.Position = cameraPosition + new Vector3(0, 0, _settings.FaceCameraHeightOffset);

            _bodyCamera = CameraHelper.CreateCamera(_settings.BodyCamera);
            _faceCamera = CameraHelper.CreateCamera(_settings.FaceCamera);
            _enterCamera = CameraHelper.CreateCamera(_settings.EnterCamera);
        }

        private void DeleteCameras()
        {
            CameraHelper.DestroyCamera(_bodyCamera);
            CameraHelper.DestroyCamera(_faceCamera);
            CameraHelper.DestroyCamera(_enterCamera);
        }

        private async Task PlayEnterCameraAnimation()
        {
            API.SetCamActive(_enterCamera, true);
            API.RenderScriptCams(true, false, 0, true, true);

            API.SetCamActiveWithInterp(_bodyCamera, _enterCamera, CustomizerSettings.EnterCameraAnimTime, 1, 1);

            API.DoScreenFadeIn(CustomizerSettings.EnterAnimFadeInTime);
            await Delay(CustomizerSettings.EnterCameraAnimTime);
        }
        
        private void SwitchCamera(int camTo, int camFrom)
        {
            API.SetCamActiveWithInterp(camTo, camFrom, CustomizerSettings.SwitchBodyToFaceCameraAnimTime, 1, 1);
        }

        private async Task SetGender(string gender)
        {
            ClientData.Character.Gender = gender;
            ClientData.Character.Resemblance = 1.0f - ClientData.Character.Resemblance;
            ClientData.Character.Skintone = 1.0f - ClientData.Character.Skintone;
            await RefreshModel();
        }

        private static void RefreshPedHead()
        {
            API.SetPedHeadBlendData(API.PlayerPedId(), ClientData.Character.Mom, ClientData.Character.Dad, 0,
                ClientData.Character.Mom, ClientData.Character.Dad, 0,
                ClientData.Character.Resemblance, ClientData.Character.Skintone, 0, true);
        }

        private static void RefreshPedHair()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedComponentVariation(playerPed, 2, ClientData.Character.Hair, 0, 2);
            API.SetPedHairColor(playerPed, ClientData.Character.HairColor1, 0);

            API.ClearPedDecorations(playerPed);

            Dictionary<int, KeyValuePair<string, string>> hairDecor = null;
            switch (ClientData.Character.Gender)
            {
                case FemaleGender:
                    hairDecor = Variants.FemaleHairDecor;
                    break;
                case MaleGender:
                    hairDecor = Variants.MaleHairDecor;
                    break;
            }

            if (hairDecor != null)
            {
                var drawableHairVariation = API.GetPedDrawableVariation(playerPed, 2); // 2 - Hair
                var pair = hairDecor[drawableHairVariation];
                var collection = (uint)API.GetHashKey(pair.Key);
                var overlay = (uint)API.GetHashKey(pair.Value);
                API.AddPedDecorationFromHashes(playerPed, collection, overlay);
            }
            else
            {
                var collection = (uint)API.GetHashKey(Variants.HairDecorDefault.Key);
                var overlay = (uint)API.GetHashKey(Variants.HairDecorDefault.Value);
                API.AddPedDecorationFromHashes(playerPed, collection, overlay);
            }
        }

        private static void RefreshLipstick()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 8, ClientData.Character.Lipstick1, ClientData.Character.Lipstick2);
            API.SetPedHeadOverlayColor(playerPed, 8, 1, ClientData.Character.Lipstick3, 0);
        }

        private static void RefreshPedEyebrows()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 2, ClientData.Character.Eyebrows, ClientData.Character.Eyebrows2);
            API.SetPedHeadOverlayColor(playerPed, 2, 1, ClientData.Character.Eyebrows3, 0);
        }

        private static void RefreshAging()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 3, ClientData.Character.Age1, ClientData.Character.Age2);
        }

        private static void RefreshBlemishes()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 11, ClientData.Character.Bodyb1, ClientData.Character.Bodyb2);
        }

        private static void RefreshSunDamage()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 7, ClientData.Character.Sun1, ClientData.Character.Sun2);
        }

        private static void RefreshComplexion()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 6, ClientData.Character.Complexion1, ClientData.Character.Complexion2);
        }

        private static void RefreshMoleFreckle()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 9, ClientData.Character.Moles1, ClientData.Character.Moles2);
        }

        private static void RefreshEyeColor()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedEyeColor(playerPed, ClientData.Character.EyeColor);
        }

        private static void RefreshMakeup()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 4, ClientData.Character.Makeup1, ClientData.Character.Makeup2);
            API.SetPedHeadOverlayColor(playerPed, 4, 1, ClientData.Character.Makeup3, 0);
        }

        private static void RefreshPedBeard()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 1, ClientData.Character.Beard, ClientData.Character.Beard2);
            API.SetPedHeadOverlayColor(playerPed, 1, 1, ClientData.Character.Beard3, 0);
        }

        private static void RefreshBlush()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedHeadOverlay(playerPed, 5, ClientData.Character.Blush1, ClientData.Character.Blush2);
            API.SetPedHeadOverlayColor(playerPed, 5, 2, ClientData.Character.Blush3, 0);
        }

        private static void ChangeComponents()
        {
            var playerPed = API.PlayerPedId();
            API.SetPedDefaultComponentVariation(playerPed);

            RefreshPedHead();
            RefreshPedHair();
            RefreshLipstick();
            RefreshPedEyebrows();
            RefreshAging();
            RefreshBlemishes();
            RefreshSunDamage();
            RefreshComplexion();
            RefreshMoleFreckle();
            RefreshEyeColor();
            RefreshMakeup();

            API.SetPedFaceFeature(playerPed, 19, ClientData.Character.NeckThick);
            API.SetPedFaceFeature(playerPed, 18, ClientData.Character.ChinHole);
            API.SetPedFaceFeature(playerPed, 17, ClientData.Character.ChinWidth);
            API.SetPedFaceFeature(playerPed, 16, ClientData.Character.ChinLength);
            API.SetPedFaceFeature(playerPed, 15, ClientData.Character.ChinHeight);
            API.SetPedFaceFeature(playerPed, 14, ClientData.Character.Jaw2);
            API.SetPedFaceFeature(playerPed, 13, ClientData.Character.Jaw1);
            API.SetPedFaceFeature(playerPed, 12, ClientData.Character.LipsThick);
            API.SetPedFaceFeature(playerPed, 11, ClientData.Character.EyeOpen);
            API.SetPedFaceFeature(playerPed, 10, ClientData.Character.Cheeks3);
            API.SetPedFaceFeature(playerPed, 9, ClientData.Character.Cheeks2);
            API.SetPedFaceFeature(playerPed, 8, ClientData.Character.Cheeks1);
            API.SetPedFaceFeature(playerPed, 6, ClientData.Character.Eyebrows6);
            API.SetPedFaceFeature(playerPed, 7, ClientData.Character.Eyebrows5);
            API.SetPedFaceFeature(playerPed, 5, ClientData.Character.Nose6);
            API.SetPedFaceFeature(playerPed, 4, ClientData.Character.Nose5);
            API.SetPedFaceFeature(playerPed, 3, ClientData.Character.Nose4);
            API.SetPedFaceFeature(playerPed, 2, ClientData.Character.Nose3);
            API.SetPedFaceFeature(playerPed, 1, ClientData.Character.Nose2);
            API.SetPedFaceFeature(playerPed, 0, ClientData.Character.Nose1);

            if (ClientData.Character.Gender == MaleGender)
            {
                SetPedOutfit(playerPed, Variants.MaleDefaultOutfits);
                RefreshPedBeard();
            }
            else if (ClientData.Character.Gender == FemaleGender)
            {
                // var outfitIndex = ClientData.CharacterData.Outfit;
                // var outfit = _femaleOutfits[outfitIndex];
                SetPedOutfit(playerPed, Variants.FemaleDefaultOutfits);
            }

            RefreshBlush();

            if (ClientData.Character.Glasses == 0)
            {
                if (ClientData.Character.Gender == MaleGender)
                {
                    API.SetPedPropIndex(playerPed, 1, 11, 0, false);
                }
                else
                {
                    API.SetPedPropIndex(playerPed, 1, 5, 0, false);
                }
            }
            else
            {
                if (ClientData.Character.Gender == MaleGender)
                {
                    API.SetPedPropIndex(playerPed, 1, 5, 0, false);
                }
                else
                {
                    API.SetPedPropIndex(playerPed, 1, 11, 0, false);
                }
            }
        }

        private static void SetPedOutfit(int playerPed, Dictionary<int, KeyValuePair<int, int>> outfit)
        {
            foreach (var pair in outfit)
            {
                var componentId = pair.Key;
                var drawableId = pair.Value.Key;
                var textureId = pair.Value.Value;
                API.SetPedComponentVariation(playerPed, componentId, drawableId, textureId, 2);
            }
        }

        private static uint GetModelHash()
        {
            if (ClientData.Character.Gender == FemaleGender)
            {
                return (uint)PedHash.FreemodeFemale01;
            }

            return (uint)PedHash.FreemodeMale01;
        }
    }
}