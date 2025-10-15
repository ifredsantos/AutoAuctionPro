using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Application.Services;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using AutoAuctionPro.Infrastructure;
using System.Threading.Tasks;

namespace AutoAuctionPro.Tests
{
    public class AuctionServiceFullCycleTests
    {
        private readonly AppDbContext _db;

        private VehicleRepository _vehicleRepo;
        private AuctionRepository _auctionRepo;

        public AuctionServiceFullCycleTests()
        {
            _db = DbConnection.CreatePostgresDb(); // Use CreateInMemoryDb() for SQLite in-memory or CreatePostgresDb() for PostgreSQL

            _vehicleRepo = new VehicleRepository(_db);
            _auctionRepo = new AuctionRepository(_db);

            DatabaseSeeder.SeedVehicles(_db);
        }

        [Fact]
        public async Task TestAllFlow()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);
            VehicleService vehicleService = new VehicleService(_vehicleRepo);

            var car = new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 15000, 5);
            await vehicleService.AddAsync(car);

            await auctionService.StartAuctionAsync(car.Id);

            await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 17000);
            await auctionService.PlaceBidAsync(car.Id, "Ana Maria", 18000);
            await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 19500);
            await auctionService.PlaceBidAsync(car.Id, "João Simões", 20000);
            await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 21000);

            Auction auctionClosed = await auctionService.CloseAuctionAsync(car.Id);

            Assert.Equal("Frederico Santos", auctionClosed.WinnerBidder);
            Assert.Equal(21000, auctionClosed.AmountSold);
        }

        [Fact]
        public async Task TestWithoutBids()
        {
            using (_db)
            {
                VehicleRepository vehicleRepo = new VehicleRepository(_db);
                AuctionRepository auctionRepo = new AuctionRepository(_db);

                AuctionService auctionService = new AuctionService(vehicleRepo, auctionRepo);
                VehicleService vehicleService = new VehicleService(vehicleRepo);

                IEnumerable<Vehicle> searchResult = await vehicleService.GetAllAsync(new VehicleSearchCriteria(VehicleType.SUV));

                if (searchResult != null && searchResult.Count() > 0)
                {
                    Vehicle car = searchResult.FirstOrDefault()!;

                    await auctionService.StartAuctionAsync(car.Id);

                    Auction auctionClosed = await auctionService.CloseAuctionAsync(car.Id);

                    Assert.Null(auctionClosed.WinnerBidder);
                    Assert.Null(auctionClosed.AmountSold);
                }
            }
        }
    }
}
