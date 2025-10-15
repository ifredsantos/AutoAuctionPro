using AutoAuctionPro.Domain.Exceptions;
using System.Security.Cryptography;

namespace AutoAuctionPro.Domain.Entities
{
    public class Auction
    {
        private readonly List<Bid> _bids = new List<Bid>();

        public Guid Id { get; set; }
        public string VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal StartingBid { get; set; }
        public bool IsActive { get; set; }
        public DateTime? OpenDateUTC { get; set; }
        public DateTime? CloseDateUTC { get; set; }
        public decimal? AmountSold { get; set; }
        public string? WinnerBidder { get; set; }

        public IReadOnlyList<Bid> Bids => _bids.AsReadOnly();
        public decimal CurrentHighestBid => _bids.Any() ? _bids.Max(b => b.Amount) : StartingBid;

        public Auction(string vehicleId, decimal startingBid)
        {
            VehicleId = vehicleId;
            StartingBid = startingBid;
        }

        public void Start()
        {
            if (IsActive)
                throw new AuctionAlreadyActiveException(Id.ToString());
            IsActive = true;
            OpenDateUTC = DateTime.UtcNow;
        }

        public void PlaceBid(Bid bid)
        {
            if (!IsActive)
                throw new AuctionNotActiveException(Id.ToString());

            if (bid.Amount <= CurrentHighestBid)
                throw new InvalidBidException($"Bid amount must be higher than current highest bid {CurrentHighestBid}");

            _bids.Add(bid);
        }

        public Bid? Close()
        {
            if (!IsActive)
                throw new AuctionNotActiveException(Id.ToString());

            Bid? bidReference = _bids?.OrderByDescending(b => b.Amount)?.FirstOrDefault();

            IsActive = false;
            CloseDateUTC = DateTime.UtcNow;
            WinnerBidder = bidReference?.BidderName;
            AmountSold = bidReference?.Amount;

            if (!string.IsNullOrEmpty(WinnerBidder))
                Vehicle.IsSold = true;

            return bidReference;
        }
    }
}
