using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Exceptions;

namespace AutoAuctionPro.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException("Missing " + nameof(vehicleRepository));
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException("It is necessary to fill in the " + nameof(vehicle));

            if (await _vehicleRepository.ExistsAsync(vehicle.Id))
                throw new DuplicateVehicleException(vehicle.Id);

            return await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task<Vehicle?> GetByIdAsync(string id)
        {
            return await _vehicleRepository.GetByIdAsync(id) ?? throw new AuctionNotFoundException(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(VehicleSearchCriteria filterCriteria)
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var query = allVehicles.AsQueryable();

            if (filterCriteria != null)
            {
                if (filterCriteria.Type.HasValue)
                    query = query.Where(v => v.Type == filterCriteria.Type.Value);

                if (!string.IsNullOrWhiteSpace(filterCriteria.Manufacture))
                    query = query.Where(v => v.Manufacturer.Equals(filterCriteria.Manufacture, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(filterCriteria.Model))
                    query = query.Where(v => v.Model.Equals(filterCriteria.Model, StringComparison.OrdinalIgnoreCase));

                if (filterCriteria.Year.HasValue)
                    query = query.Where(v => v.Year == filterCriteria.Year.Value);

                if (filterCriteria.isSold.HasValue)
                    query = query.Where(v => v.IsSold == filterCriteria.isSold.Value);
            }

            return query.ToList();
        }
    }
}
