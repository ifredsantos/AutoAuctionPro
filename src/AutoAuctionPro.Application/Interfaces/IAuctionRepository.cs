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
        Auction? GetActiveByVehicleId(string vehicleId);
        void Remove(string vehicleId);
        IEnumerable<Auction> GetAll();
    }
}
