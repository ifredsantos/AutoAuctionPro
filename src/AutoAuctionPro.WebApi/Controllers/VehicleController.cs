using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoAuctionPro.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IAuctionService _auctionService;

        public VehicleController(IVehicleService vehicleService, IAuctionService auctionService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException("Missing " + nameof(vehicleService));
            _auctionService = auctionService ?? throw new ArgumentNullException("Missing " + nameof(auctionService));
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAll(VehicleSearchCriteria filterCriteria)
        {
            return Ok(null);
        }

        // GET: api/vehicles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetById(string id)
        {
            return Ok(null);
        }
    }
}
