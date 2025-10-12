using AutoAuctionPro.Application.Interfaces;

namespace AutoAuctionPro.WebApi.Middlewares
{
    public class BidderMiddleware
    {
        private readonly RequestDelegate _next;

        public BidderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBidderRepository bidders)
        {
            if (context.Request.Headers.TryGetValue("X-Bidder-Username", out var username))
            {
                var bidder = bidders.GetOrCreate(username);
                context.Items["Bidder"] = bidder;
            }

            await _next(context);
        }
    }

}
