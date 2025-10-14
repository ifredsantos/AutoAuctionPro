using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Services;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using AutoAuctionPro.Infrastructure;

namespace AutoAuctionPro.Tests
{
    public class AuctionServiceTests
    {
        private readonly AppDbContext _db;

        private VehicleRepository _vehicleRepo;
        private AuctionRepository _auctionRepo;
        private BidRepository _bidRepo;

        public AuctionServiceTests()
        {
            _db = DbConnection.CreatePostgresDb(); // Use CreateInMemoryDb() for SQLite in-memory or CreatePostgresDb() for PostgreSQL

            DatabaseSeeder.SeedVehicles(_db);

            _vehicleRepo = new VehicleRepository(_db);
            _auctionRepo = new AuctionRepository(_db);
            _bidRepo = new BidRepository(_db);
        }

        #region Main Methods

        [Fact]
        public void TestAddVehicle()
        {
            var car = new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 15000, 5);
        }

        [Fact]
        public void TestStartAuction()
        {

        }

        [Fact]
        public void TestPlaceBid()
        {

        }

        [Fact]
        public void TestCloseAuction()
        {

        }

        #endregion

        #region Expected errors

        [Fact]
        public void TestDuplicateVehicle()
        {

        }

        [Fact]
        public void TestVehicleNotFound()
        {

        }

        [Fact]
        public void TestVehicleIsAlreadySold()
        {

        }

        [Fact]
        public void TestAuctionAlreadyActive()
        {

        }

        [Fact]
        public void TestAuctionInactive()
        {

        }

        [Fact]
        public void TestAuctionNotFound()
        {

        }

        #endregion

        #region Other situations

        [Fact]
        public async Task TestAllFlow()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo, _bidRepo);
            VehicleService vehicleService = new VehicleService(_vehicleRepo);

            var car = new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 15000, 5);
            await vehicleService.AddAsync(car);

            await auctionService.StartAuctionAsync(car.Id);

            await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 17000);
            await auctionService.PlaceBidAsync(car.Id, "Ana Maria", 18000);
            await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 19500);
            await auctionService.PlaceBidAsync(car.Id, "João Simões", 20000);
            await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 21000);

            var result = await auctionService.CloseAuctionAsync(car.Id);

            Assert.Equal("Frederico Santos", result.Winner);
            Assert.Equal(21000, result.Amount);
        }

        [Fact]
        public async Task TestWithoutBids()
        {
            using (_db)
            {
                VehicleRepository vehicleRepo = new VehicleRepository(_db);
                AuctionRepository auctionRepo = new AuctionRepository(_db);
                BidRepository bidRepo = new BidRepository(_db);

                AuctionService auctionService = new AuctionService(vehicleRepo, auctionRepo, bidRepo);
                VehicleService vehicleService = new VehicleService(vehicleRepo);

                IEnumerable<Vehicle> searchResult = await vehicleService.GetAllAsync(new VehicleSearchCriteria(VehicleType.SUV));

                if (searchResult != null && searchResult.Count() > 0)
                {
                    Vehicle car = searchResult.FirstOrDefault();

                    await auctionService.StartAuctionAsync(car.Id);

                    var result = await auctionService.CloseAuctionAsync(car.Id);

                    Assert.Null(result.Winner);
                    Assert.Null(result.Amount);
                }
            }
        }

        #endregion
    }
}
