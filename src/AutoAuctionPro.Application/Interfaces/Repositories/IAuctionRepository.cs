using AutoAuctionPro.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task<Auction?> GetActiveByVehicleIdAsync(string vehicleId);
        Task RemoveAsync(Guid id);
        Task<IEnumerable<Auction>> GetAllAsync();
    }
}
