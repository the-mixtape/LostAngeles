using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace LostAngeles.Client.Core
{
    public class CameraSettings
    {
        public string Name;
        public Vector3 Position;
        public float Fov;

        public CameraSettings(string name, Vector3 position, float fov)
        {
            Position = position;
            Fov = fov;
            Name = name;
        }
    }

    public class CameraHelper
    {
        public static int CreateCamera(CameraSettings settings, bool active = false)
        {
            return API.CreateCamWithParams(
                settings.Name,
                settings.Position.X, settings.Position.Y, settings.Position.Z,
                0, 0, 0,
                settings.Fov, active, 0);
        }

        public static void DestroyCamera(int camera, bool scriptHostCam = false)
        {
            API.DestroyCam(camera, scriptHostCam);
        }
    }
}