using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Infrastructure
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _db;
        public VehicleRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("Missing " + nameof(db));
        }

        public void Add(Vehicle vehicle)
        {
            if (_db.Vehicles.Any(v => v.Id == vehicle.Id))
                throw new InvalidOperationException($"Vehicle with id {vehicle.Id} already exists.");

            _db.Vehicles.Add(vehicle);

            _db.SaveChanges();
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _db.Vehicles.AsNoTracking().ToList();
        }

        public Vehicle? GetById(string id)
        {
            return _db.Vehicles.Find(id);
        }

        public bool Exists(string id)
        {
            return _db.Vehicles.Any(v => v.Id == id);
        }
    }
}
