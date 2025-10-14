using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task<Auction?> GetActiveByVehicleIdAsync(string vehicleId);
        Task RemoveAsync(Guid id);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<Auction> GetByVehicleIdAsync(string vehicleId);
    }
}
