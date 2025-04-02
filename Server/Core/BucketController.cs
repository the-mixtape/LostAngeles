using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Shared;
using NLog;

namespace LostAngeles.Server.Core
{
    public class BucketController : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("BUCKETCONTROLLER");
        private static readonly HashSet<int> UsedBuckets = new HashSet<int>();

        public BucketController()
        {
            EventHandlers[ServerEvents.BucketController.SetBucketEvent] += new Action<CitizenFX.Core.Player, int, int>(OnSetBucket);
        }
        
        private int GetUnusedBucketId()
        {
            ClearEmptyBuckets();
            int newBucket = 1;
            while (UsedBuckets.Contains(newBucket))
            {
                newBucket++;
            }
            UsedBuckets.Add(newBucket);
            return newBucket;
        }

        private void FreeBucket(int bucketId)
        {
            Log.Info("Free Bucket: " + bucketId);
            UsedBuckets.Remove(bucketId);
        }

        private void ClearEmptyBuckets()
        {
            HashSet<int> used = new HashSet<int>();
            foreach (CitizenFX.Core.Player player in Players)
            {
                if(player == null || player.Character == null) continue;
                var id = API.GetPlayerRoutingBucket(player.Character.Handle.ToString());
                used.Add(id);
            }
            
            var forDelete = new HashSet<int>(UsedBuckets);
            forDelete.ExceptWith(used);

            foreach (var id in forDelete)
            {
                FreeBucket(id);
            }
        }

        private int GetBucketIdForType(RoutingBucketTypes type)
        {
            if (type == RoutingBucketTypes.Uniq)
            {
                return GetUnusedBucketId();
            }

            return 0;
        }
        
        private void OnSetBucket([FromSource] CitizenFX.Core.Player source, int netId, int bucketType)
        {
            var licenseIdentifier = Helper.GetLicense(source);
            
            var entityId = API.NetworkGetEntityFromNetworkId(netId);
            var type = (RoutingBucketTypes)bucketType;

            var id = GetBucketIdForType(type);
            API.SetPlayerRoutingBucket(entityId.ToString(), id);
            Log.Info($"Player ({source.Name}|{licenseIdentifier}) was moved to Bucket#{id}");
        }
    }
}