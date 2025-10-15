using AutoAuctionPro.Domain.Enums;

namespace AutoAuctionPro.Application.DTOs
{
    public record VehicleSearchCriteria(VehicleType? Type = null, string? Manufacture = null, string? Model = null, int? Year = null, bool? isSold = null);
}
