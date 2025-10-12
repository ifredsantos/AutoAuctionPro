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

        public async Task StartAuctionAsync(string vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId) ?? throw new VehicleNotFoundException(vehicleId);
            
            var existingAuction = await _auctionRepository.GetActiveByVehicleIdAsync(vehicleId);
            if(existingAuction != null)
                throw new AuctionAlreadyActiveException(vehicleId);

            var auction = new Auction(vehicleId, vehicle.StartingBid);
            auction.Start();

            await _auctionRepository.AddAsync(auction);
        }

        public async Task PlaceBidAsync(string vehicleId, string bidder, decimal amount)
        {
            if(string.IsNullOrEmpty(bidder))
                throw new ArgumentException("Bidder name is required", nameof(bidder));

            var auction = await _auctionRepository.GetActiveByVehicleIdAsync(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            var bid = new Bid(auction.Id, bidder, amount);

            try
            {
                auction.PlaceBid(bid);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidBidException(ex.Message);
            }

            await _bidRepository.AddAsync(bid);
        }

        public async Task<(string? Winner, decimal? Amount)> CloseAuctionAsync(string vehicleId)
        {
            var auction = await _auctionRepository.GetActiveByVehicleIdAsync(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            var bid = auction.Close();

            await _auctionRepository.UpdateAsync(auction);

            return (bid?.BidderName, bid?.Amount);
        }

        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _auctionRepository.GetAllAsync();
        }
    }
}
