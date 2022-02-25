using System.Collections.Generic;
using FlightPlaner.Archive;
using FlightPlaner.Exception;
using FlightPlaner.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors]
    public class CustomerController : ControllerBase
    {
        private readonly FlightPlanerDbContext _context;

        public CustomerController(FlightPlanerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            List<Airport> flight = FlightStorage.searchAirports(_context,search);
            return Ok(flight);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest req)
        {
            if (FlightStorage.CheckInvalidAirportSearch(req))
            {
                return BadRequest();
            }

            PageResult flight = FlightStorage.SearchFlight(_context,req);
            return Ok(flight);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlight(int id)
        {
            try
            {
                var flight = FlightStorage.GetFlight(_context,id);
                return Ok(flight);
            }
            catch (InvalidIdException)
            {
                return NotFound();
            }
        }
    }
}
