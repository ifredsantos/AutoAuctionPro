using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Infrastructure
{
    public class BidderRepository : IBidderRepository
    {
        private readonly AppDbContext _db;

        public BidderRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("Missing " + nameof(db));
        }

        public async Task<Bidder> GetOrCreateAsync(string bidderUsername)
        {
            Bidder bidder = new Bidder(bidderUsername);

            await _db.Bidders.AddAsync(bidder);
            await _db.SaveChangesAsync();

            return bidder;
        }
    }
}
