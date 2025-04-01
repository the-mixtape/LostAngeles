using System;

namespace LostAngeles.Server.Domain
{
    public class Blacklist
    {
        public string License { get; set; }
        public DateTimeOffset BlockedAt { get; set; }
        public string Reason { get; set; }

        public override string ToString()
        {
            return $"{License}, {BlockedAt}, {Reason}";
        }
    }
}