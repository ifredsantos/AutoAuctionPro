namespace AutoAuctionPro.Domain.Entities
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; init; }


        public Hatchback(string? manufacturer, string? model, int year, decimal startingBid, int numberOfDoors, string? id = null)
        : base(Enums.VehicleType.Hatchback, manufacturer, model, year, startingBid, id)
        {
            if (numberOfDoors <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors));

            NumberOfDoors = numberOfDoors;
        }
    }
}
