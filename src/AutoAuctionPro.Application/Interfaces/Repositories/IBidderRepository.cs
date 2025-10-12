using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IBidderRepository
    {
        Task<Bidder> GetOrCreateAsync(string bidderUsername);
    }
}
