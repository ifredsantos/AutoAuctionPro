namespace AutoAuctionPro.Domain.Entities
{
    public class Bid
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        public string BidderName { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTimeUTC { get; set; }


        public Bid(Guid auctionId, string bidderName, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(bidderName))
                throw new ArgumentException("bidder name is required", nameof(bidderName));

            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Id = Guid.NewGuid();
            AuctionId = auctionId;
            BidderName = bidderName;
            Amount = amount;
            BidTimeUTC = DateTime.UtcNow;
        }
    }
}
