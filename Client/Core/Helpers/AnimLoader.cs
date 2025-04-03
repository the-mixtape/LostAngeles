using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace LostAngeles.Client.Core
{
    public class AnimLoader : BaseScript
    {
        public static async Task LoadAnimDict(string dict)
        {
            API.RequestAnimDict(dict);
            while (!API.HasAnimDictLoaded(dict))
            {
                await Delay(0);
            }
        }

        public static async Task LoadClipSet(string clipset)
        {
            API.RequestClipSet(clipset);
            while (!API.HasClipSetLoaded(clipset))
            {
                await Delay(0);
            }
        }
    }
}