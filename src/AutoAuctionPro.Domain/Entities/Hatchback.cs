using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Entities
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; init; }


        public Hatchback(string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
        : base(Enums.VehicleType.Hatchback, manufacturer, model, year, startingBid)
        {
            if (numberOfDoors <= 0) 
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors));

            NumberOfDoors = numberOfDoors;
        }
    }
}
