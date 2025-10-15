using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Application.Services;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using AutoAuctionPro.Infrastructure;
using System.Threading.Tasks;

namespace AutoAuctionPro.Tests
{
    public class AuctionServiceTests
    {
        private readonly AppDbContext _db;

        private VehicleRepository _vehicleRepo;
        private AuctionRepository _auctionRepo;

        public AuctionServiceTests()
        {
            _db = DbConnection.CreatePostgresDb(); // Use CreateInMemoryDb() for SQLite in-memory or CreatePostgresDb() for PostgreSQL

            DatabaseSeeder.SeedVehicles(_db);

            _vehicleRepo = new VehicleRepository(_db);
            _auctionRepo = new AuctionRepository(_db);
        }

        #region Main Methods

        [Fact]
        public async Task TestAddVehicle()
        {
            VehicleService vehicleService = new VehicleService(_vehicleRepo);

            Vehicle vehicle = new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 15000, 5);

            Vehicle addedVehicle = await vehicleService.AddAsync(vehicle);

            Assert.NotNull(addedVehicle);
            Assert.Equal(vehicle.Manufacturer, addedVehicle.Manufacturer);
            Assert.Equal(vehicle.Model, addedVehicle.Model);
        }

        [Fact]
        public async Task TestStartAuction()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);
            VehicleService vehicleService = new VehicleService(_vehicleRepo);

            IEnumerable<Vehicle> searchResult = await vehicleService.GetAllAsync(new VehicleSearchCriteria(isSold: false));
            Vehicle vehicle = searchResult?.FirstOrDefault()!;

            if (vehicle != null)
            {
                Auction auction = await auctionService.StartAuctionAsync(vehicle.Id);

                Assert.NotNull(auction);
                Assert.Equal(vehicle.Id, auction.VehicleId);
                Assert.True(auction.IsActive);
            }
        }

        [Fact]
        public async Task TestPlaceBid()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            IEnumerable<Auction> activeAuctionList = (await auctionService.GetAllAsync())?.Where(x => x.CloseDateUTC == null)!;
            Auction? activeAuction = null;

            if (activeAuctionList != null && activeAuctionList.Count() == 0)
            {
                activeAuction = await auctionService.GetByVehicleIdAsync(activeAuctionList.FirstOrDefault()!.VehicleId);
            }

            if (activeAuction != null)
            {
                string bidder = "Frederico Santos";
                decimal amount = activeAuction.CurrentHighestBid + 5000;

                Bid addedBid = await auctionService.PlaceBidAsync(activeAuction.VehicleId, bidder, amount);

                Assert.NotNull(addedBid);
                Assert.Equal(bidder, addedBid.BidderName);
                Assert.Equal(amount, addedBid.Amount);
            }
        }

        [Fact]
        public async Task TestCloseAuction()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            IEnumerable<Auction> activeAuctionList = (await auctionService.GetAllAsync())?.Where(x => x.CloseDateUTC == null)!;

            Auction? activeAuction = activeAuctionList?.FirstOrDefault();

            if (activeAuction != null)
            {
                Auction auctionClosed = await auctionService.CloseAuctionAsync(activeAuction.VehicleId);

                Assert.NotNull(auctionClosed.WinnerBidder);
            }
        }

        #endregion

        #region Expected errors

        [Fact]
        public async Task TestDuplicateVehicle()
        {
        }

        [Fact]
        public async Task TestVehicleNotFound()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            await auctionService.StartAuctionAsync("ADAMASTOR_1234321");
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

        #endregion
    }
}
