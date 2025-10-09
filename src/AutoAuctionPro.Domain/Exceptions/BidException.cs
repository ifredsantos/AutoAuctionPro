using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public class InvalidBidException : Exception
    {
        public InvalidBidException(string message) : base(message) { }
    }
}
