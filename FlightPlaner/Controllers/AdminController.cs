using FlightPlaner.Archive;
using FlightPlaner.Exception;
using FlightPlaner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Route("Flights/{id}")]
        public IActionResult GetFlights(int id)
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

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult PutFlights(AddFlightRequest request)
        {
            try
            {
                var flight = FlightStorage.AddFlight(request);
                return Created("", flight);
            }
            catch (DuplicateDataException)
            {
                return Conflict();
            }
            catch (NullOrEmptyException)
            {
                return BadRequest();
            }
            catch (DateTimeException)
            {
                return BadRequest();
            }
            catch (DuplicateAirportException)
            {
                return BadRequest();
            }
        }

        [HttpDelete, Authorize]
        [Route("flights/{id}")]
        public IActionResult RemoveFlights(int id)
        {
            FlightStorage.DeleteFlights(id);
            return Ok();
        }
    }
}
