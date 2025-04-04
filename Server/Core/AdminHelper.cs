using System.Collections.Generic;

namespace LostAngeles.Server.Core
{
    public class AdminHelper
    {
        private static HashSet<string> _admins;

        public static void Initialize(HashSet<string> admins)
        {
            _admins = admins;
        }

        public static bool IsAdmin(CitizenFX.Core.Player player)
        {
            var license = Helper.GetLicense(player);
            return IsAdmin(license);
        }
        
        public static bool IsAdmin(string license)
        {
            return _admins != null && _admins.Contains(license);
        }
    }
}