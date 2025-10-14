using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Domain.Exceptions
{
    public abstract class DomainExceptionBase : Exception, IDomainException
    {
        protected DomainExceptionBase(int statusCode, string title, string message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }

        public int StatusCode { get; }
        public string Title { get; }
    }
}
