using AutoAuctionPro.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Interfaces
{
    public interface IBidRepository
    {
        Task AddAsync(Bid bid);
    }
}
