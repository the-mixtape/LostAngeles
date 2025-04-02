using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;

namespace LostAngeles.Client.Core
{
    public class BucketController : BaseScript
    {
        private static RoutingBucketTypes _currentBucket = RoutingBucketTypes.Default;

        public static void SetLocalPLayerRoutingBucket(RoutingBucketTypes bucket)
        {
            if (_currentBucket == bucket) return;

            _currentBucket = bucket;
            var netId = API.GetPlayerServerId(API.PlayerId());
            TriggerServerEvent(ServerEvents.BucketController.SetBucketEvent, netId, (int)bucket);
        }
    }
}