using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new InvalidOperationException("Auction is already active");
            IsActive = true;
            OpenDateUTC = DateTime.UtcNow;
        }

        public void PlaceBid(Bid bid)
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is not active");
            
            if (bid.Amount <= CurrentHighestBid)
                throw new ArgumentOutOfRangeException(nameof(bid), $"Bid amount must be higher than current highest bid {CurrentHighestBid}");

            _bids.Add(bid);
        }

        public Bid? Close()
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is not active");

            IsActive = false;
            CloseDateUTC = DateTime.UtcNow;

            return _bids.OrderByDescending(b => b.Amount).FirstOrDefault();
        }
    }
}
