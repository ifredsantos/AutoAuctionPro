using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public class AuctionAlreadyActiveException : Exception
    {
        public AuctionAlreadyActiveException(string id) : base($"Auction already active for vehicle '{id}'.") { }
    }

    public class AuctionNotActiveException : Exception
    {
        public AuctionNotActiveException(string id) : base($"No active auction for vehicle '{id}'.") { }
    }
}
