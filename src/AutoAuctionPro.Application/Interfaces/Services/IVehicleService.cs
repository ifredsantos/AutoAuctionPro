using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IVehicleService
    {
        Task AddAsync(Vehicle vehicle);
        Task<Vehicle?> GetByIdAsync(string id);
        Task<IEnumerable<Vehicle>> GetAllAsync(VehicleSearchCriteria filterCriteria);
    }
}
