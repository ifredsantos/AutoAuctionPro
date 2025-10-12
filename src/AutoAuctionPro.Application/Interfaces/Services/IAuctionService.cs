using AutoAuctionPro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IAuctionService
    {
        Task StartAuctionAsync(string vehicleId);
        Task PlaceBidAsync(string vehicleId, string bidder, decimal amount);
        Task<(string? Winner, decimal? Amount)> CloseAuctionAsync(string vehicleId);
        Task<IEnumerable<Auction>> GetAllAsync();
    }
}
