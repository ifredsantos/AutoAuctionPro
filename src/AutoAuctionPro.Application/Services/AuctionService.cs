using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Exceptions;

namespace AutoAuctionPro.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException("Missing " + nameof(vehicleRepository));
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException("Missing " + nameof(auctionRepository));
        }

        public async Task<Auction> StartAuctionAsync(string vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId) ?? throw new VehicleNotFoundException(vehicleId);

            if (vehicle.IsSold)
                throw new VehicleIsAlreadySoldException(vehicle.Id);

            var existingAuction = await _auctionRepository.GetActiveByVehicleIdAsync(vehicleId);
            if (existingAuction != null)
                throw new AuctionAlreadyActiveException(vehicleId);

            var auction = new Auction(vehicleId, vehicle.StartingBid);
            auction.Start();

            return await _auctionRepository.AddAsync(auction);
        }

        public async Task<Bid> PlaceBidAsync(string vehicleId, string bidder, decimal amount)
        {
            if (string.IsNullOrEmpty(bidder))
                throw new ArgumentException("Bidder name is required", nameof(bidder));

            var auction = await _auctionRepository.GetActiveByVehicleIdAsync(vehicleId)
                ?? throw new AuctionNotActiveException(vehicleId);
            var bid = new Bid(auction.Id, bidder, amount);

            auction.PlaceBid(bid);

            return await _auctionRepository.PlaceBidAsync(bid);
        }

        public async Task<Auction> CloseAuctionAsync(string vehicleId)
        {
            var auction = await _auctionRepository.GetActiveByVehicleIdAsync(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            Bid? bid = auction.Close();

            await _auctionRepository.UpdateAsync(auction);

            return auction;
        }

        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _auctionRepository.GetAllAsync();
        }

        public async Task<Auction> GetByVehicleIdAsync(string vehicleId)
        {
            return await _auctionRepository.GetByVehicleIdAsync(vehicleId) ?? throw new AuctionNotFoundException(vehicleId);
        }
    }
}
