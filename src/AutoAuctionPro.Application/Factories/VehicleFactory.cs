using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AutoAuctionPro.Application.Factories
{
    public class VehicleFactory : IVehicleFactory
    {
        public Vehicle CreateVehicle(VehicleType type, string id, string manufacturer, string model, int year, decimal startingBid, int? doors = null, int? seats = null, double? capacity = null)
        {
            Vehicle vehicle = null;

            switch (type)
            {
                case VehicleType.Sedan:
                    vehicle = new Sedan(id, manufacturer, model, year, startingBid, doors ?? 5);
                    break;
                case VehicleType.Hatchback:
                    vehicle = new Hatchback(id, manufacturer, model, year, startingBid, doors ?? 5);
                    break;
                case VehicleType.SUV:
                    vehicle = new SUV(id, manufacturer, model, year, startingBid, seats ?? 5);
                    break;
                case VehicleType.Truck:
                    vehicle = new Truck(id, manufacturer, model, year, startingBid, capacity ?? 1000);
                    break;
                default:
                    throw new ArgumentException("Unsupported vehicle type", nameof(type));
            }

            return vehicle;
        }
    }
}
