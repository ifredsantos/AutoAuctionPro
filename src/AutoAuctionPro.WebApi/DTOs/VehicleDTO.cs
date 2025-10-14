using AutoAuctionPro.Domain.Enums;

namespace AutoAuctionPro.WebApi.DTOs
{
    public class VehicleDTO
    {
        public string Id { get; set; }
        public VehicleType Type { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }
        public bool IsSold { get; set; }

        // Specific properties for different vehicle types
        public int NumberOfDoors { get; set; }
        public int NumberOfSeats { get; set; }
        public double LoadCapacityKg { get; set; }
    }
}
