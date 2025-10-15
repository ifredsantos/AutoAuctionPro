using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> AddAsync(Auction auction);
        Task<Auction> UpdateAsync(Auction auction);
        Task<Auction?> GetActiveByVehicleIdAsync(string vehicleId);
        Task RemoveAsync(Guid id);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<Auction?> GetByVehicleIdAsync(string vehicleId);
        Task<Bid> PlaceBidAsync(Bid bid);
    }
}
