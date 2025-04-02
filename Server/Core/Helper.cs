namespace LostAngeles.Server.Core
{
    public static class Helper
    {
        public static string GetLicense(CitizenFX.Core.Player player)
        {
            return player.Identifiers["license"];
        }
    }
}