using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException("Missing " + nameof(vehicleRepository));
        }

        public void Add(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException("It is necessary to fill in the " + nameof(vehicle));

            if (_vehicleRepository.Exists(vehicle.Id))
                throw new DuplicateVehicleException(vehicle.Id);

            _vehicleRepository.Add(vehicle);
        }

        public Vehicle? GetById(string id)
        {
            return _vehicleRepository.GetById(id);
        }

        public IEnumerable<Vehicle> GetAll(VehicleSearchCriteria filterCriteria)
        {
            var allVehicles = _vehicleRepository.GetAll();
            var query = allVehicles.AsQueryable();

            if (filterCriteria.Type.HasValue)
                query = query.Where(v => v.Type == filterCriteria.Type.Value);

            if (!string.IsNullOrWhiteSpace(filterCriteria.Manufacture))
                query = query.Where(v => v.Manufacturer.Equals(filterCriteria.Manufacture, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filterCriteria.Model))
                query = query.Where(v => v.Model.Equals(filterCriteria.Model, StringComparison.OrdinalIgnoreCase));

            if (filterCriteria.Year.HasValue)
                query = query.Where(v => v.Year == filterCriteria.Year.Value);

            return query.ToList();
        }
    }
}
