using System.Linq;
using System.Threading.Tasks;
using FlightPlaner.Archive;
using FlightPlaner.Exception;
using FlightPlaner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlaner.Controllers
{
    [Route("admin-api")]
    [EnableCors("MyPolicy")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private static readonly object requestLock = new object();
        private readonly FlightPlanerDbContext _dbStorageContext;

        public AdminController(FlightPlanerDbContext context)
        {
            _dbStorageContext = context;
        }

        [HttpGet]
        [Route("Flights/{id}")]
        public async Task<IActionResult> GetFlights(int id)
        {

            var flight = await _dbStorageContext.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .SingleOrDefaultAsync(f=>f.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);

            
        }

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult PutFlights(AddFlightRequest request)
        {
            lock (requestLock)
            {
                try
                {
                    var flight = FlightStorage.ConvertToFlight(request);
                    CheckDuplicateFlight(flight);
                    _dbStorageContext.Flights.Add(flight);
                    _dbStorageContext.SaveChanges();

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
        }

        [HttpDelete, Authorize]
        [Route("flights/{id}")]
        public IActionResult RemoveFlights(int id)
        {
            lock (requestLock)
            {
                var flight = _dbStorageContext.Flights
                    .Include(f => f.From)
                    .Include(f => f.To)
                    .SingleOrDefault(f => f.Id == id);
                if (flight != null)
                {
                    _dbStorageContext.Flights.Remove(flight);
                    _dbStorageContext.SaveChanges();
                }

                return Ok();
            }
        }


        private bool CheckDuplicateFlight(Flight flight)
        {
            
                if (_dbStorageContext.Flights
                    .Any(fly => fly.To.AirportName.Trim().ToLower() == flight.To.AirportName.Trim().ToLower() &&
                                                 fly.To.City.Trim().ToLower() == flight.To.City.Trim().ToLower() &&
                                                 fly.To.Country.Trim().ToLower() == flight.To.Country.Trim().ToLower() &&
                                                 fly.From.City.Trim().ToLower() == flight.From.City.Trim().ToLower() &&
                                                 fly.From.Country.Trim().ToLower() == flight.From.Country.Trim().ToLower() &&
                                                 fly.From.AirportName.Trim().ToLower() == flight.From.AirportName.Trim().ToLower() &&
                                                 fly.ArrivalTime.Trim().ToLower() == flight.ArrivalTime.Trim().ToLower() &&
                                                 fly.Carrier.Trim().ToLower() == flight.Carrier.Trim().ToLower() &&
                                                 fly.DepartureTime.Trim().ToLower() == flight.DepartureTime.Trim().ToLower()))
                {
                    throw new DuplicateDataException();
                }

                return false;
            
        }
    }
}