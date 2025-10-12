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
    public class BidderRepository : IBidderRepository
    {
        private readonly AppDbContext _db;

        public BidderRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException("Missing " + nameof(db));
        }

        public Bidder GetOrCreate(string bidderUsername)
        {
            Bidder bidder = new Bidder(bidderUsername);

            _db.Bidders.Add(bidder);
            _db.SaveChanges();

            return bidder;
        }
    }
}
