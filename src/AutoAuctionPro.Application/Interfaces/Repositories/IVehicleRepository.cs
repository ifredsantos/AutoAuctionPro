using AutoAuctionPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
