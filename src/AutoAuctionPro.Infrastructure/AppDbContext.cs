
using AutoAuctionPro.Domain.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasKey(v => v.Id);
                b.Property(v => v.Manufacturer).IsRequired();
                b.Property(v => v.Model).IsRequired();
                b.Property(v => v.Year).IsRequired();
                b.Property(v => v.StartingBid);
                //Optei por usar o conceito Table Per Hierarchy para simplificar o mapeamento das subclasses
                //desta forma evito criar várias tabelas uma para cada tipo
                b.Property<string>("Discriminator").HasMaxLength(20);
                b.HasDiscriminator<string>("Discriminator")
                    .HasValue<Sedan>("Sedan")
                    .HasValue<Hatchback>("Hatchback")
                    .HasValue<SUV>("SUV")
                    .HasValue<Truck>("Truck");
                b.Property<DateTime>("CreatedAt").HasDefaultValueSql("NOW()");
            });

            modelBuilder.Entity<Sedan>().Property<int>("NumberOfDoors");
            modelBuilder.Entity<Hatchback>().Property<int>("NumberOfDoors");
            modelBuilder.Entity<SUV>().Property<int>("NumberOfSeats");
            modelBuilder.Entity<Truck>().Property<double>("LoadCapacityKg");

            modelBuilder.Entity<Auction>(b =>
            {
                b.HasKey(a => a.VehicleId);
                b.Property(a => a.StartingBid);
                b.Property(a => a.IsActive);
                b.Ignore(a => a.Bids);
                b.Property<DateTime>("CreatedAt").HasDefaultValueSql("NOW()");
            });
        }
    }
}
