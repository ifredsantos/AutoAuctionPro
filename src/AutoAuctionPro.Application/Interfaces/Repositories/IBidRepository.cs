using AutoAuctionPro.Domain.Entities;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IBidRepository
    {
        Task AddAsync(Bid bid);
    }
}
