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

    public class VehicleInvalidAmountException : DomainExceptionBase
    {
        public VehicleInvalidAmountException(int minValue) : base(400, "Invalid amount value", $"The vehicle has an invalid amount value. It must be greater than {minValue}.") { }
    }

    public class VehicleInvalidYearException : DomainExceptionBase
    {
        public VehicleInvalidYearException(int minYear, int maxYear) : base(400, "Invalid year", $"The vehicle's year value is invalid. It must be between {minYear} and {maxYear}.") { }
    }
}
