using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoAuctionPro.Infrastructure
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _db;

        public VehicleRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("Missing " + nameof(db));
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            if (await _db.Vehicles.AnyAsync(v => v.Id == vehicle.Id))
                throw new InvalidOperationException($"Vehicle with id {vehicle.Id} already exists.");

            var entry = await _db.Vehicles.AddAsync(vehicle);

            await _db.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            var entry = _db.Vehicles.Update(vehicle);

            await _db.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _db.Vehicles.AsNoTracking().ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(string id)
        {
            return await _db.Vehicles.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _db.Vehicles.AnyAsync(v => v.Id == id);
        }
    }
}
