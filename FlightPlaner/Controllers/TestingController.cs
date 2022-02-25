using Microsoft.AspNetCore.Mvc;
using FlightPlaner.Archive;
using Microsoft.AspNetCore.Cors;

namespace FlightPlaner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    [EnableCors]
    public class TestingController : ControllerBase
    {
        private readonly FlightPlanerDbContext _context;

        public TestingController(FlightPlanerDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("Clear")]
        public IActionResult Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
            return Ok();
        }
    }
}