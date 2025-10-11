using AutoAuctionPro.Domain.ValueObjects;
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

        public string? VehicleId { get; set; }
        public decimal StartingBid { get; set; }
        public bool IsActive { get; set; }
        public IReadOnlyList<Bid> Bids => _bids.AsReadOnly();
        public decimal CurrentHighestBid => _bids.Any() ? _bids.Max(b => b.Amount) : StartingBid;

        public Auction(string? vehicleId, decimal startingBid)
        {
            VehicleId = vehicleId;
            StartingBid = startingBid;
            IsActive = true;
        }

        public void Start()
        {
            if (IsActive)
                throw new InvalidOperationException("Auction is already active");
            IsActive = true;
        }

        public void PlaceBid(Bid bid)
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is not active");
            
            if (bid.Amount <= CurrentHighestBid)
                throw new ArgumentOutOfRangeException(nameof(bid), "Bid amount must be higher than current highest bid");

            _bids.Add(bid);
        }

        public Bid? Close()
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is not active");

            IsActive = false;

            return _bids.OrderByDescending(b => b.Amount).FirstOrDefault();
        }
    }
}
