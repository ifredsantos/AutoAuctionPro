namespace AutoAuctionPro.Domain.Exceptions
{
    public class InvalidBidException : DomainExceptionBase
    {
        public InvalidBidException(string message) : base(400, "Invalid Bid", message) { }
    }
}
