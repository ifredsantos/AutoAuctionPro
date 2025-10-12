using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Exceptions;

namespace AutoAuctionPro.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;

        public AuctionService(IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository, IBidRepository bidRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException("Missing " + nameof(vehicleRepository));
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException("Missing " + nameof(auctionRepository));
            _bidRepository = bidRepository;
        }

        public void StartAuction(string vehicleId)
        {
            var vehicle = _vehicleRepository.GetById(vehicleId) ?? throw new VehicleNotFoundException(vehicleId);
            
            var existingAuction = _auctionRepository.GetActiveByVehicleId(vehicleId);
            if(existingAuction != null)
                throw new AuctionAlreadyActiveException(vehicleId);

            var auction = new Auction(vehicleId, vehicle.StartingBid);
            auction.Start();

            _auctionRepository.Add(auction);
        }

        public void PlaceBid(string vehicleId, string bidder, decimal amount)
        {
            if(string.IsNullOrEmpty(bidder))
                throw new ArgumentException("Bidder name is required", nameof(bidder));

            var auction = _auctionRepository.GetActiveByVehicleId(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            var bid = new Bid(auction.Id, bidder, amount);

            try
            {
                auction.PlaceBid(bid);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidBidException(ex.Message);
            }

            _bidRepository.Add(bid);
        }

        public (string? Winner, decimal? Amount) CloseAuction(string vehicleId)
        {
            var auction = _auctionRepository.GetActiveByVehicleId(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            var bid = auction.Close();

            _auctionRepository.Update(auction);

            return (bid?.BidderName, bid?.Amount);
        }

        public IEnumerable<Auction> GetAll()
        {
            return _auctionRepository.GetAll();
        }
    }
}
