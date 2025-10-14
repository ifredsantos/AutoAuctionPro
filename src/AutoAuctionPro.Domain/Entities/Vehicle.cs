using AutoAuctionPro.Domain.Enums;

namespace AutoAuctionPro.Domain.Entities
{
    /// <summary>
    /// Represents a generic vehicle that can be listed in an auction.
    /// </summary>
    /// <remarks>
    /// This abstract base class defines common properties and validation rules
    /// for all vehicle types (e.g., Sedan, SUV).
    /// </remarks>
    public abstract class Vehicle
    {
        /// <summary>
        /// Unique identifier for the vehicle.
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Type of the vehicle (e.g., Sedan, SUV).
        /// </summary>
        public VehicleType Type { get; }
        /// <summary>
        /// Vehicle manufacturer (e.g., BMW, Mercedes).
        /// </summary>
        public string Manufacturer { get; }
        /// <summary>
        /// Specific model of the vehicle (e.g., CLA45, E36).
        /// </summary>
        public string Model { get; }
        /// <summary>
        /// Manufacturing year of the vehicle.
        /// Must be between 1800 and the current year.
        /// </summary>
        public int Year { get; }
        /// <summary>
        /// Starting bid amount for the auction.
        /// Must be non-negative.
        /// </summary>
        public decimal StartingBid { get; }
        /// <summary>
        /// Indicates whether the vehicle has already been sold
        /// </summary>
        public bool IsSold { get; set; }

        public List<Auction> Auction { get; } = new List<Auction>();


        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class with validation.
        /// </summary>
        /// <param name="id">Unique vehicle identifier.</param>
        /// <param name="type">Type of the vehicle.</param>
        /// <param name="manufacturer">Vehicle manufacturer.</param>
        /// <param name="model">Vehicle model name.</param>
        /// <param name="year">Year of manufacture.</param>
        /// <param name="startingBid">Initial bid amount.</param>
        /// <exception cref="ArgumentException">Thrown when id, manufacturer or model is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when year or startingBid are out of valid range.</exception>
        protected Vehicle(VehicleType type, string manufacturer, string model, int year, decimal startingBid)
        {
            if (string.IsNullOrWhiteSpace(manufacturer))
                throw new ArgumentException("manufacturer is required", nameof(manufacturer));

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("model is required", nameof(model));

            if (year < 1800 || year > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(nameof(year));

            if (startingBid < 0)
                throw new ArgumentOutOfRangeException(nameof(startingBid));


            Type = type;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            Id = GenerateCustomId();
        }

        private string GenerateCustomId()
        {
            string manufacturerPart = (Manufacturer?.Length > 5 ? Manufacturer.Substring(0, 5) : Manufacturer ?? string.Empty);
            string modelPart = (Model?.Length > 5 ? Model.Substring(0, 5) : Model ?? string.Empty);

            return $"{manufacturerPart}-{modelPart}-{Year}-{Guid.NewGuid().ToString().Substring(0, 8)}".ToUpper();
        }

    }
}
