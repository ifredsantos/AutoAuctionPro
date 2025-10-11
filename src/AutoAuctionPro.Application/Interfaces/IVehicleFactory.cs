using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IVehicleFactory
    {
        Vehicle CreateVehicle(VehicleType type, string id, string manufacturer, string model, int year, decimal startingBid, int? doors = null, int? seats = null, double? capacity = null);
    }
}
