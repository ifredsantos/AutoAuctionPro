using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Infrastructure;

namespace AutoAuctionPro.Tests
{
    public static partial class DatabaseSeeder
    {
        public static void SeedVehicles(AppDbContext db)
        {
            if(db.Vehicles.Any())
                return;

            List<Vehicle> vehicles = new List<Vehicle>
            {
                new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 20000, 5),
                new Sedan("Honda", "Accord", 2020, 20000m, 4),
                new Hatchback("Ford", "Focus", 2019, 15000m, 4),
                new SUV("Chevrolet", "Tahoe", 2021, 35000m, 7),
                new Truck("Ford", "F-150", 2018, 30000m, 1000)
            };

            db.Vehicles.AddRange(vehicles);
            db.SaveChanges();
        }
    }
}
