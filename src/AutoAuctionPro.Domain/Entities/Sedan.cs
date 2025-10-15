namespace AutoAuctionPro.Domain.Entities
{
    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; init; }


        public Sedan(string? manufacturer, string? model, int year, decimal startingBid, int numberOfDoors, string? id = null)
        : base(Enums.VehicleType.Sedan, manufacturer, model, year, startingBid, id)
        {
            if (numberOfDoors <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors));

            NumberOfDoors = numberOfDoors;
        }
    }
}
