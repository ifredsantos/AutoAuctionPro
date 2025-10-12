using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Entities
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; init; }


        public SUV(string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
        : base(Enums.VehicleType.SUV, manufacturer, model, year, startingBid)
        {
            if (numberOfSeats <= 0) 
                throw new ArgumentOutOfRangeException(nameof(numberOfSeats));


            NumberOfSeats = numberOfSeats;
        }
    }
}
