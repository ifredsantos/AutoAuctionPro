using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IBidderRepository
    {
        Bidder GetOrCreate(string bidderUsername);
    }
}
