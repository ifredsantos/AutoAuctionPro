using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public class DuplicateVehicleException : DomainExceptionBase
    {
        public DuplicateVehicleException(string id) : base(409, "Vehicle already exists", $"Vehicle with id '{id}' already exists.") { }
    }

    public class VehicleNotFoundException : DomainExceptionBase
    {
        public VehicleNotFoundException(string id) : base(404, "Vehicle not found", $"Vehicle with id '{id}' not found.") { }
    }

    public class VehicleIsAlreadySoldException : DomainExceptionBase
    {
        public VehicleIsAlreadySoldException(string id) : base(409, "Vehicle already sold", $"Vehicle with id '{id}' is already sold and cannot be auctioned again.") { }
    }
}
