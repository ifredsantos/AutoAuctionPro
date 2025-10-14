using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IAuctionService
    {
        Task<Auction> StartAuctionAsync(string vehicleId);
        Task PlaceBidAsync(string vehicleId, string bidder, decimal amount);
        Task<(string? Winner, decimal? Amount)> CloseAuctionAsync(string vehicleId);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<Auction> GetByVehicleIdAsync(string vehicleId);
    }
}
