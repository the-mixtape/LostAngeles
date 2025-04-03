namespace LostAngeles.Server.Domain
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Heading { get; set; }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {Heading})";
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string License { get; set; }
        public string Character { get; set; }
        public Position Position { get; set; }
    }
}