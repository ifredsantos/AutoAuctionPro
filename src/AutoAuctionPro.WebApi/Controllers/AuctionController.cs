using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Application.Services;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Domain.Exceptions;
using AutoAuctionPro.WebApi.DTOs;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AutoAuctionPro.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuctionService _auctionService;

        public AuctionController(IMapper mapper, IAuctionService auctionService)
        {
            _mapper = mapper ?? throw new ArgumentNullException("Missing " + nameof(mapper));
            _auctionService = auctionService ?? throw new ArgumentNullException("Missing " + nameof(auctionService));
        }

        // GET: api/auction
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var auctions = await _auctionService.GetAllAsync();
            List<AuctionBaseDTO> auctionsDTO = _mapper.Map<List<AuctionBaseDTO>>(auctions);
            return Ok(auctionsDTO);
        }

        // GET: api/auction/{vehicleId}
        [HttpGet("{vehicleId}")]
        public async Task<ActionResult> GetByVehicleIdAsync(string vehicleId)
        {
            Auction auction = await _auctionService.GetByVehicleIdAsync(vehicleId);
            AuctionDTO auctionDTO = _mapper.Map<AuctionDTO>(auction);
            return Ok(auctionDTO);
        }

        // POST: api/auction/{vehicleId}/start
        [HttpPost("{vehicleId}/start")]
        public async Task<ActionResult> StartAuction(string vehicleId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Auction auction = await _auctionService.StartAuctionAsync(vehicleId);
            AuctionBaseDTO auctionDTO = _mapper.Map<AuctionBaseDTO>(auction);
            return Ok(auctionDTO);
        }

        // POST: api/auction/{vehicleId}/bid
        [HttpPost("{vehicleId}/bid")]
        public async Task<ActionResult> PlaceBid(string vehicleId, [FromBody] PlaceBidRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _auctionService.PlaceBidAsync(vehicleId, request.Bidder, request.Amount);
            return Ok(new { message = "Bid placed successfully." });
        }

        // POST: api/auction/{vehicleId}/close
        [HttpPost("{vehicleId}/close")]
        public async Task<ActionResult> CloseAuction(string vehicleId)
        {
            var (winner, amount) = await _auctionService.CloseAuctionAsync(vehicleId);
            return Ok(new { winner, amount });
        }
    }
}
