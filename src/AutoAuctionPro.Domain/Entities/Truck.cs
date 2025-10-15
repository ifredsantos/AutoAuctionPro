namespace AutoAuctionPro.Domain.Entities
{
    public class Truck : Vehicle
    {
        public double LoadCapacityKg { get; init; }


        public Truck(string? manufacturer, string? model, int year, decimal startingBid, double loadCapacityKg, string? id = null)
        : base(Enums.VehicleType.Truck, manufacturer, model, year, startingBid, id)
        {
            if (loadCapacityKg <= 0)
                throw new ArgumentOutOfRangeException(nameof(loadCapacityKg));

            LoadCapacityKg = loadCapacityKg;
        }
    }
}
