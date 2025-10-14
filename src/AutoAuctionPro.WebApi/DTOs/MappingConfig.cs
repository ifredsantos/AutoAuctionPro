using AutoAuctionPro.Domain.Entities;
using Mapster;

namespace AutoAuctionPro.WebApi.DTOs
{
    public static class MappingConfig
    {
        public static TypeAdapterConfig RegisterMappings()
        {
            var config = new TypeAdapterConfig();

            config.NewConfig<Vehicle, VehicleDTO>()
                .Map(dest => dest.Type, src => src.GetType().Name)
                .AfterMapping((src, dest) =>
                {
                    switch (src)
                    {
                        case Sedan s:
                            dest.NumberOfDoors = s.NumberOfDoors;
                            break;
                        case Hatchback h:
                            dest.NumberOfDoors = h.NumberOfDoors;
                            break;
                        case SUV suv:
                            dest.NumberOfSeats = suv.NumberOfSeats;
                            break;
                        case Truck t:
                            dest.LoadCapacityKg = t.LoadCapacityKg;
                            break;
                    }
                });

            config.NewConfig<Bid, BidDTO>();

            config.NewConfig<Auction, AuctionDTO>()
                .Map(dest => dest.Vehicle, src => src.Vehicle)
                .Map(dest => dest.Bids, src => src.Bids);

            return config;
        }
    }
}
