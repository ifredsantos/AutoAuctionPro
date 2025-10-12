using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Infrastructure
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _db;

        public AuctionRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("Missing " + nameof(db));
        }

        public async Task AddAsync(Auction auction)
        {
            await _db.Auctions.AddAsync(auction);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _db.Auctions.Update(auction);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _db.Auctions.AsNoTracking().ToListAsync();
        }

        public async Task<Auction?> GetActiveByVehicleIdAsync(string vehicleId)
        {
            return await _db.Auctions.Include(a => a.Bids).AsTracking().FirstOrDefaultAsync(x => x.VehicleId == vehicleId && x.IsActive);
        }

        public async Task RemoveAsync(Guid id)
        {
            var auction = _db.Auctions.Find(id);
            if (auction != null)
            {
                _db.Auctions.Remove(auction);
                await _db.SaveChangesAsync();
            }
        }
    }
}
