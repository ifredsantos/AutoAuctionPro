using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Services;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using AutoAuctionPro.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace AutoAuctionPro.Tests
{
    public class AuctionServiceTests
    {
        private readonly AppDbContext _db;

        public AuctionServiceTests()
        {
            _db = DbConnection.CreatePostgresDb(); // Use CreateInMemoryDb() for SQLite in-memory or CreatePostgresDb() for PostgreSQL

            DatabaseSeeder.SeedVehicles(_db);
        }

        [Fact]
        public void TestAllFlow()
        {
            using (_db)
            {
                VehicleRepository vehicleRepo = new VehicleRepository(_db);
                AuctionRepository auctionRepo = new AuctionRepository(_db);
                BidRepository bidRepo = new BidRepository(_db);

                AuctionService auctionService = new AuctionService(vehicleRepo, auctionRepo, bidRepo);
                VehicleService vehicleService = new VehicleService(vehicleRepo);

                var car = new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 15000, 5);
                vehicleService.Add(car);

                auctionService.StartAuction(car.Id);

                auctionService.PlaceBid(car.Id, "Frederico Santos", 17000);
                auctionService.PlaceBid(car.Id, "Ana Maria", 18000);
                auctionService.PlaceBid(car.Id, "Frederico Santos", 19500);
                auctionService.PlaceBid(car.Id, "João Simões", 20000);
                auctionService.PlaceBid(car.Id, "Frederico Santos", 21000);

                var result = auctionService.CloseAuction(car.Id);

                string? winner = result.Winner;
                decimal? amount = result.Amount;
            }
        }


        [Fact]
        public void TestWithoutBids()
        {
            using (_db)
            {
                VehicleRepository vehicleRepo = new VehicleRepository(_db);
                AuctionRepository auctionRepo = new AuctionRepository(_db);
                BidRepository bidRepo = new BidRepository(_db);

                AuctionService auctionService = new AuctionService(vehicleRepo, auctionRepo, bidRepo);
                VehicleService vehicleService = new VehicleService(vehicleRepo);

                IEnumerable<Vehicle> searchResult = vehicleService.GetAll(new VehicleSearchCriteria(VehicleType.SUV));

                if (searchResult != null && searchResult.Count() > 0)
                {
                    Vehicle car = searchResult.FirstOrDefault();

                    auctionService.StartAuction(car.Id);

                    var result = auctionService.CloseAuction(car.Id);

                    string? winner = result.Winner;
                    decimal? amount = result.Amount;
                }
            }
        }
    }
}
