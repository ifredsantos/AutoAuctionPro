namespace AutoAuctionPro.WebApi.DTOs
{
    public class AuctionBaseDTO
    {
        public Guid Id { get; set; }
        public VehicleDTO? Vehicle { get; set; }
        public decimal StartingBid { get; set; }
        public bool IsActive { get; set; }
        public DateTime? OpenDateUTC { get; set; }
        public DateTime? CloseDateUTC { get; set; }
        public decimal? AmountSold { get; set; }
        public string? WinnerBidder { get; set; }
    }

    public class AuctionDTO : AuctionBaseDTO
    {
        public List<BidDTO>? Bids { get; set; }
    }

    public class BidDTO
    {
        public Guid Id { get; set; }
        public string? BidderName { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTimeUTC { get; set; }
    }

    public class PlaceBidRequest
    {
        public string? Bidder { get; set; }
        public decimal Amount { get; set; }
    }
}
