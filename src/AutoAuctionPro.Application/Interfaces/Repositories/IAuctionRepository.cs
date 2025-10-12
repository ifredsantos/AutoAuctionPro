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
        void Add(Auction auction);
        void Update(Auction auction);
        Auction? GetActiveByVehicleId(string vehicleId);
        void Remove(Guid id);
        IEnumerable<Auction> GetAll();
    }
}
