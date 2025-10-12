using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Entities
{
    public class Bidder
    {
        public string? Username { get; set; }


        public Bidder(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            Username = username;
        }
    }
}
