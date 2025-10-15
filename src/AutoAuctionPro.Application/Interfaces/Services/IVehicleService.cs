using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<Vehicle?> GetByIdAsync(string id);
        Task<IEnumerable<Vehicle>> GetAllAsync(VehicleSearchCriteria filterCriteria);
    }
}
