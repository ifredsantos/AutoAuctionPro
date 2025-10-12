
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Vehicle configuration
            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasKey(v => v.Id);
                b.Property(v => v.Manufacturer).IsRequired();
                b.Property(v => v.Model).IsRequired();
                b.Property(v => v.Year).IsRequired();
                b.Property(v => v.StartingBid);
                //Optei por usar o conceito Table Per Hierarchy para simplificar o mapeamento das subclasses
                //desta forma evito criar várias tabelas uma para cada tipo
                b.HasDiscriminator(v => v.Type)
                    .HasValue<Sedan>(VehicleType.Sedan)
                    .HasValue<Hatchback>(VehicleType.Hatchback)
                    .HasValue<SUV>(VehicleType.SUV)
                    .HasValue<Truck>(VehicleType.Truck);
            });

            modelBuilder.Entity<Sedan>().Property<int>("NumberOfDoors");
            modelBuilder.Entity<Hatchback>().Property<int>("NumberOfDoors");
            modelBuilder.Entity<SUV>().Property<int>("NumberOfSeats");
            modelBuilder.Entity<Truck>().Property<double>("LoadCapacityKg");


            //Auction configuration
            modelBuilder.Entity<Auction>(b =>
            {
                b.HasKey(a => a.Id);
                b.Property(a => a.StartingBid).IsRequired();
                b.Property(a => a.IsActive);
                b.Property(a => a.OpenDateUTC);
                b.Property(a => a.CloseDateUTC);
                b.Ignore(a => a.CurrentHighestBid);
                
                b.HasMany(a => a.Bids)
                    .WithOne(bid => bid.Auction)
                    .HasForeignKey(bid => bid.AuctionId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(a => a.Vehicle)
                    .WithMany(v => v.Auction)
                    .HasForeignKey(a => a.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            //Bid configuration
            modelBuilder.Entity<Bid>(b =>
            {
                b.HasKey(bid => bid.Id);
                b.Property(bid => bid.BidderName).IsRequired();
                b.Property(bid => bid.Amount).IsRequired();
                b.Property(bid => bid.BidTimeUTC).IsRequired();
            });
        }
    }
}
