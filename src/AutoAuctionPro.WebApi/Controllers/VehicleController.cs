using AutoAuctionPro.Application.DTOs;
using AutoAuctionPro.Application.Interfaces;
using AutoAuctionPro.Domain.Entities;
using AutoAuctionPro.WebApi.DTOs;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace AutoAuctionPro.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVehicleService _vehicleService;

        public VehicleController(IMapper mapper, IVehicleService vehicleService)
        {
            _mapper = mapper ?? throw new ArgumentNullException("Missing " + nameof(mapper));
            _vehicleService = vehicleService ?? throw new ArgumentNullException("Missing " + nameof(vehicleService));
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetAll([FromBody] VehicleSearchCriteria filterCriteria = null)
        {
            var vehicles = await _vehicleService.GetAllAsync(filterCriteria);
            IEnumerable<VehicleDTO> vehiclesDTO = _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);

            return Ok(vehiclesDTO);
        }

        // GET: api/vehicles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDTO>> GetById(string id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound($"Vehicle with ID {id} not found");

            VehicleDTO vehicleDTO = _mapper.Map<VehicleDTO>(vehicle);

            return Ok(vehicleDTO);
        }

        // POST: api/vehicles
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] VehicleDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Vehicle vehicle = null;

            switch (request.Type)
            {
                case Domain.Enums.VehicleType.Sedan:
                    vehicle = new Sedan(request.Manufacturer, request.Model, request.Year, request.StartingBid, request.NumberOfDoors);
                    break;
                case Domain.Enums.VehicleType.SUV:
                    vehicle = new SUV(request.Manufacturer, request.Model, request.Year, request.StartingBid, request.NumberOfSeats);
                    break;
                case Domain.Enums.VehicleType.Hatchback:
                    vehicle = new Hatchback(request.Manufacturer, request.Model, request.Year, request.StartingBid, request.NumberOfDoors);
                    break;
                case Domain.Enums.VehicleType.Truck:
                    vehicle = new Truck(request.Manufacturer, request.Model, request.Year, request.StartingBid, request.LoadCapacityKg);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.Type), "Invalid vehicle type");
            }

            await _vehicleService.AddAsync(vehicle);

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
    }
}
