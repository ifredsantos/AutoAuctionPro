using AutoAuctionPro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.DTOs
{
    public record VehicleSearchCriteria(VehicleType? Type = null, string? Manufacture = null, string? Model = null, int? Year = null);
}
