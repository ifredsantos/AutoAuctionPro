namespace AutoAuctionPro.Domain.Entities
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; init; }


        public SUV(string? manufacturer, string? model, int year, decimal startingBid, int numberOfSeats, string? id = null)
        : base(Enums.VehicleType.SUV, manufacturer, model, year, startingBid, id)
        {
            if (numberOfSeats <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfSeats));


            NumberOfSeats = numberOfSeats;
        }
    }
}
