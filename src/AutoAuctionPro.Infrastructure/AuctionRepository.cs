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

        public void Add(Auction auction)
        {
            _db.Auctions.Add(auction);
            _db.SaveChanges();
        }

        public IEnumerable<Auction> GetAll()
        {
            return _db.Auctions.AsNoTracking().ToList();
        }

        public Auction? GetActiveByVehicleId(string vehicleId)
        {
            return _db.Auctions.FirstOrDefault(x => x.VehicleId == vehicleId && x.IsActive);
        }

        public void Remove(string vehicleId)
        {
            var auction = _db.Auctions.Find(vehicleId);
            if (auction != null)
            {
                _db.Auctions.Remove(auction);
                _db.SaveChanges();
            }
        }
    }
}
