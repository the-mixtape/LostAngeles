using CitizenFX.Core;

namespace LostAngeles.Shared
{
    /// <summary>
    /// Represents spawn information, including coordinates and heading for an entity.
    /// </summary>
    public class PlayerPosition
    {
        /// <summary>
        /// Gets or sets of the spawn location.
        /// </summary>
        public Vector3 Location { get; set; }

        /// <summary>
        /// Gets or sets the heading (rotation) of the entity at the spawn location, in degrees.
        /// </summary>
        public float Heading { get; set; }

        public PlayerPosition()
        {
            this.Location = Vector3.Zero;
            this.Heading = 0f;
        }

        public PlayerPosition(float x, float y, float z, float heading)
        {
            this.Location = new Vector3(x, y, z);
            this.Heading = heading;
        }

        public override string ToString()
        {
            return $"({this.Location.X}, {this.Location.Y}, {this.Location.Z}, {this.Heading})";
        }

        public bool Compare(PlayerPosition other, float accuracy = 0.01f)
        {
            if (other == null)
            {
                return false;
            }
            
            return System.Math.Abs(Location.X - other.Location.X) < accuracy &&
                   System.Math.Abs(Location.Y - other.Location.Y) < accuracy &&
                   System.Math.Abs(Location.Z - other.Location.Z) < accuracy &&
                   System.Math.Abs(Heading - other.Heading) < accuracy;
        }
    }
}