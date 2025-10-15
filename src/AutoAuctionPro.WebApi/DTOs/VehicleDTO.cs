using AutoAuctionPro.Domain.Enums;

namespace AutoAuctionPro.WebApi.DTOs
{
    public class VehicleBaseDTO
    {
        public string? Id { get; set; }
        public VehicleType Type { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }

        // Specific properties for different vehicle types
        public int NumberOfDoors { get; set; }
        public int NumberOfSeats { get; set; }
        public double LoadCapacityKg { get; set; }
    }

    public class VehicleDTO : VehicleBaseDTO
    {
        public bool IsSold { get; set; }
    }

    public class CreateVehicleDTO : VehicleBaseDTO
    {

    }
}
