using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Exceptions;
using AutoAuctionPro.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuctionPro.Application.Services
{
    public class AuctionService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException("Missing " + nameof(vehicleRepository));
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException("Missing " + nameof(auctionRepository));
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if(vehicle == null)
                throw new ArgumentNullException("It is necessary to fill in the " + nameof(vehicle));

            if(_vehicleRepository.Exists(vehicle.Id))
                throw new DuplicateVehicleException(vehicle.Id);

            _vehicleRepository.Add(vehicle);
        }

        public IEnumerable<Vehicle> Search(VehicleSearchCriteria filterCriteria)
        {
            var allVehicles = _vehicleRepository.GetAll();
            var query = allVehicles.AsQueryable();

            if(filterCriteria.Type.HasValue)
                query = query.Where(v => v.Type == filterCriteria.Type.Value);

            if(!string.IsNullOrWhiteSpace(filterCriteria.Manufacture))
                query = query.Where(v => v.Manufacturer.Equals(filterCriteria.Manufacture, StringComparison.OrdinalIgnoreCase));

           if(!string.IsNullOrWhiteSpace(filterCriteria.Model))
                query = query.Where(v => v.Model.Equals(filterCriteria.Model, StringComparison.OrdinalIgnoreCase));

           if(filterCriteria.Year.HasValue)
                query = query.Where(v => v.Year == filterCriteria.Year.Value);

              return query.ToList();
        }

        public void StartAuction(string vehicleId)
        {
            var vehicle = _vehicleRepository.GetById(vehicleId) ?? throw new VehicleNotFoundException(vehicleId);
            
            var existingAuction = _auctionRepository.GetActiveByVehicleId(vehicleId);
            if(existingAuction != null)
                throw new AuctionAlreadyActiveException(vehicleId);

            var auction = new Auction(vehicleId, vehicle.StartingBid);
            auction.Start();

            _auctionRepository.Add(auction);
        }

        public void PlaceBid(string vehicleId, string bidder, decimal amount)
        {
            if(string.IsNullOrEmpty(bidder))
                throw new ArgumentException("Bidder name is required", nameof(bidder));

            var auction = _auctionRepository.GetActiveByVehicleId(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            var bid = new Bid(bidder, amount);

            try
            {
                auction.PlaceBid(bid);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidBidException(ex.Message);
            }
        }

        public (string? Winner, decimal? Amount) CloseAuction(string vehicleId)
        {
            var auction = _auctionRepository.GetActiveByVehicleId(vehicleId) ?? throw new AuctionNotActiveException(vehicleId);
            var bid = auction.Close();

            _auctionRepository.Remove(vehicleId);

            return (bid?.Bidder, bid?.Amount);
        }
    }
}
