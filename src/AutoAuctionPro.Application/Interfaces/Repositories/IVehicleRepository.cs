using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle auction);
        Task<Vehicle?> GetByIdAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
    }
}
