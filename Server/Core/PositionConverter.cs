using LostAngeles.Server.Domain;
using LostAngeles.Shared;

namespace LostAngeles.Server.Core
{
    public static class PositionConverter
    {
        public static Position ToPosition(this SpawnPosition spawnPos)
        {
            return new Position
            {
                X = spawnPos.Location.X,
                Y = spawnPos.Location.Y,
                Z = spawnPos.Location.Z,
                Heading = spawnPos.Heading
            };
        }

        public static SpawnPosition ToSpawnPosition(this Position pos)
        {
            return new SpawnPosition(pos.X, pos.Y, pos.Z, pos.Heading);
        }
    }
}