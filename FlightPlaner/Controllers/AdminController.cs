using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Models;
using FlightPlanner.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner.Controllers
{
    [Route("admin-api")]
    [EnableCors("MyPolicy")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private static readonly object requestLock = new object();
        private readonly IFlightService _flightService;
        private readonly IEnumerable<IValidator> _validators;
        private readonly IMapper _mapper;

        public AdminController(IFlightService flightService,
            IEnumerable<IValidator> validators,
            IMapper mapper)
        {
            _flightService = flightService;
            _validators = validators;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            
                var flight = _flightService.GetFlightWithAirports(id);

                return flight==null? NotFound(): (IActionResult)Ok(flight);
        }

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult PutFlights(AddFlightDto request)
        {
            lock (requestLock)
            {

                if (!_validators.All(v => v.IsValid(request)))
                    return BadRequest();
                if (_flightService.CheckDuplicateFlight(request))
                    return Conflict();

                var flight = _mapper.Map<Flight>(request);
                _flightService.Create(flight);

                return Created("", _mapper.Map<AddFlightDto>(flight));
            }
        }

        [HttpDelete, Authorize]
        [Route("flights/{id}")]
        public IActionResult RemoveFlights(int id)
        {
            lock (requestLock)
            {
                _flightService.DeleteFlightByID(id);
                return Ok();
            }
        }
    }
}