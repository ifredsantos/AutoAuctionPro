using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public interface IDomainException
    {
        int StatusCode { get; }
        string Title { get; }
    }
}
