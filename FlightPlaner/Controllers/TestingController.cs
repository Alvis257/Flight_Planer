using Microsoft.AspNetCore.Mvc;
using FlightPlaner.Archive;

namespace FlightPlaner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        [HttpPost]
        [Route("Clear")]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights();
            return Ok();
        }
    }
}