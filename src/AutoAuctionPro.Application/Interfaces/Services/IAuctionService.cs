using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IAuctionService
    {
        Task<Auction> StartAuctionAsync(string vehicleId);
        Task<Bid> PlaceBidAsync(string vehicleId, string bidder, decimal amount);
        Task<Auction> CloseAuctionAsync(string vehicleId);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<Auction> GetByVehicleIdAsync(string vehicleId);
    }
}
