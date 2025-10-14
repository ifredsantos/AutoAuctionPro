using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Infrastructure
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _db;

        public BidRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("Missing " + nameof(db));
        }

        public async Task AddAsync(Bid bid)
        {
            await _db.Bids.AddAsync(bid);
            await _db.SaveChangesAsync();
        }
    }
}
