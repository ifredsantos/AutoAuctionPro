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
        void Add(Vehicle vehicle);
        Vehicle? GetById(string id);
        IEnumerable<Vehicle> GetAll(VehicleSearchCriteria filterCriteria);
    }
}
