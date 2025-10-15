using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Application.Services;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using AutoAuctionPro.Domain.Exceptions;
using AutoAuctionPro.Infrastructure;
using Microsoft.Extensions.DependencyModel;
using System.Threading.Tasks;

namespace AutoAuctionPro.Tests
{
    public class AuctionServiceExpectedErrorTests
    {
        private readonly AppDbContext _db;

        private VehicleRepository _vehicleRepo;
        private AuctionRepository _auctionRepo;

        public AuctionServiceExpectedErrorTests()
        {
            _db = DbConnection.CreatePostgresDb(); // Use CreateInMemoryDb() for SQLite in-memory or CreatePostgresDb() for PostgreSQL

            _vehicleRepo = new VehicleRepository(_db);
            _auctionRepo = new AuctionRepository(_db);

            DatabaseSeeder.SeedVehicles(_db);
        }

        [Fact]
        public async Task TestDuplicateVehicle()
        {
            VehicleService vehicleService = new VehicleService(_vehicleRepo);

            Sedan vehicle = new Sedan("BMW", "E36 320i", 1997, 5000, 5, "BMW-E36-320i-1997");

            await vehicleService.AddAsync(vehicle);
            await Assert.ThrowsAsync<DuplicateVehicleException>(async () =>
            {
                await _vehicleRepo.AddAsync(vehicle);
            });
        }

        [Fact]
        public async Task TestVehicleNotFound()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            await Assert.ThrowsAsync<VehicleNotFoundException>(async () =>
            {
                await auctionService.StartAuctionAsync("ADAMASTOR_1234321");
            });
        }

        [Fact]
        public async Task TestVehicleIsAlreadySold()
        {
            VehicleService vehicleService = new VehicleService(_vehicleRepo);
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            IEnumerable<Vehicle> soldVehicleList = await vehicleService.GetAllAsync(new VehicleSearchCriteria { isSold = true });

            if (soldVehicleList.Any())
            {
                Vehicle? referenceVehicle = soldVehicleList.FirstOrDefault();
                await Assert.ThrowsAsync<VehicleIsAlreadySoldException>(async () =>
                {
                    await auctionService.StartAuctionAsync(referenceVehicle!.Id);
                });
            }
        }

        [Fact]
        public async Task TestAuctionAlreadyActive()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            IEnumerable<Auction> auctionList = await auctionService.GetAllAsync();
            if(auctionList.Any())
            {
                Auction? referenceAuction = auctionList.FirstOrDefault(a => a.CloseDateUTC == null);
                await Assert.ThrowsAsync<AuctionAlreadyActiveException>(async () =>
                {
                    await auctionService.StartAuctionAsync(referenceAuction!.VehicleId);
                });
            }
        }

        [Fact]
        public async Task TestAuctionInactive()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            IEnumerable<Auction> auctionList = await auctionService.GetAllAsync();
            if (auctionList.Any())
            {
                Auction? referenceAuction = auctionList.FirstOrDefault(a => a.CloseDateUTC != null);
                await Assert.ThrowsAsync<AuctionNotActiveException>(async () =>
                {
                    await auctionService.CloseAuctionAsync(referenceAuction!.VehicleId);
                });
            }
        }

        [Fact]
        public async Task TestAuctionNotFound()
        {
            AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);

            await Assert.ThrowsAsync<AuctionNotFoundException>(async () =>
            {
                await auctionService.GetByVehicleIdAsync("RANDOM ID");
            });
            
        }
    }
}
