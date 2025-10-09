using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.ValueObjects
{
    public class Bid
    {
        public string Bidder { get; }
        public decimal Amount { get; }
        public DateTime TimestampUtc { get; }


        public Bid(string bidder, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(bidder)) 
                throw new ArgumentException("bidder is required", nameof(bidder));

            if (amount < 0) 
                throw new ArgumentOutOfRangeException(nameof(amount));

            Bidder = bidder;
            Amount = amount;
            TimestampUtc = DateTime.UtcNow;
        }
    }
}
