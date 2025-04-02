using CitizenFX.Core;

namespace LostAngeles.Client.Core.CharacterCustomizer
{
    class CustomizerSettings
    {
        private const string DefaultCameraName = "DEFAULT_SCRIPTED_CAMERA";

        public readonly float BodyCameraDistance = 4.0f;
        public readonly float FaceCameraHeightOffset = 0.55f;

        public readonly CameraSettings BodyCamera =
            new CameraSettings(DefaultCameraName, new Vector3(0), 30.00f);

        public readonly CameraSettings FaceCamera =
            new CameraSettings(DefaultCameraName, new Vector3(0), 10.00f);
        
        public readonly CameraSettings EnterCamera =
            new CameraSettings(DefaultCameraName, new Vector3(0), 50.00f);

        public const int EnterCameraAnimTime = 3000;
        public const int EnterAnimFadeInTime = 1500;
        public const int ExitAnimFadeOutTime = 1000;
        public const int SwitchBodyToFaceCameraAnimTime = 250;
    }
}