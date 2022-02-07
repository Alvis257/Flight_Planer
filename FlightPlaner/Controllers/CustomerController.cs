using System.Collections.Generic;
using FlightPlaner.Archive;
using FlightPlaner.Exception;
using FlightPlaner.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            List<Airport> flight = FlightStorage.searchAirports(search);
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

            PageResult flight = FlightStorage.SearchFlight(req);
            return Ok(flight);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlight(int id)
        {
            try
            {
                var flight = FlightStorage.GetFlight(id);
                return Ok(flight);
            }
            catch (InvalidIdException)
            {
                return NotFound();
            }
        }
    }
}

