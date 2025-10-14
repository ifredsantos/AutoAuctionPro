using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public class AuctionAlreadyActiveException : DomainExceptionBase
    {
        public AuctionAlreadyActiveException(string id) : base(409, "Auction already active", $"Auction already active for vehicle '{id}'.") { }
    }

    public class AuctionNotActiveException : DomainExceptionBase
    {
        public AuctionNotActiveException(string id) : base(409, "Auction not active", $"No active auction for vehicle '{id}'.") { }
    }

    public class AuctionNotFoundException : DomainExceptionBase
    {
        public AuctionNotFoundException(string id) : base(404, "Auction not found", $"Auction '{id}' not found.") { }
    }
}
