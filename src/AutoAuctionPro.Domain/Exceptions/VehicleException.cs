using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public class DuplicateVehicleException : Exception
    {
        public DuplicateVehicleException(string id) : base($"Vehicle with id '{id}' already exists.") { }
    }

    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string id) : base($"Vehicle with id '{id}' not found.") { }
    }
}
