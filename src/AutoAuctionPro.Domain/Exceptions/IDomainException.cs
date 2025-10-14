namespace AutoAuctionPro.Domain.Exceptions
{
    public interface IDomainException
    {
        int StatusCode { get; }
        string Title { get; }
    }
}
