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
        private readonly FlightPlanerDbContext _dbStorageContext;

        public TestingController(FlightPlanerDbContext context)
        {
            _dbStorageContext = context;
        }
        [HttpPost]
        [Route("Clear")]
        public IActionResult Clear()
        {
            _dbStorageContext.Flights.RemoveRange(_dbStorageContext.Flights);
            _dbStorageContext.Airports.RemoveRange(_dbStorageContext.Airports);
            _dbStorageContext.SaveChanges();
            return Ok();
        }
    }
}