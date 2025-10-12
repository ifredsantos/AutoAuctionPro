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
        void StartAuction(string vehicleId);
        void PlaceBid(string vehicleId, string bidder, decimal amount);
        (string? Winner, decimal? Amount) CloseAuction(string vehicleId);
        IEnumerable<Auction> GetAll();
    }
    //public interface IVehicleRepository
    //{
    //    void Add(Vehicle vehicle);
    //    Vehicle? GetById(string id);
    //    bool Exists(string id);
    //    IEnumerable<Vehicle> GetAll();
    //}
}
