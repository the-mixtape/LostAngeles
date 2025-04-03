using LostAngeles.Server.Domain;
using LostAngeles.Shared;

namespace LostAngeles.Server.Core
{
    public static class PositionConverter
    {
        public static Position ToPosition(this PlayerPosition playerPos)
        {
            return new Position
            {
                X = playerPos.Location.X,
                Y = playerPos.Location.Y,
                Z = playerPos.Location.Z,
                Heading = playerPos.Heading
            };
        }

        public static PlayerPosition ToSpawnPosition(this Position pos)
        {
            return new PlayerPosition(pos.X, pos.Y, pos.Z, pos.Heading);
        }
    }
}