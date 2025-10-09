using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Entities
{
    public class Truck : Vehicle
    {
        public double LoadCapacityKg { get; init; }


        public Truck(string id, string manufacturer, string model, int year, decimal startingBid, double loadCapacityKg)
        : base(id, Enums.VehicleType.Truck, manufacturer, model, year, startingBid)
        {
            if (loadCapacityKg <= 0) 
                throw new ArgumentOutOfRangeException(nameof(loadCapacityKg));

            LoadCapacityKg = loadCapacityKg;
        }
    }
}
