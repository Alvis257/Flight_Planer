using FlightPlanner.core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace FlightPlaner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    [EnableCors]
    public class TestingController : ControllerBase
    {
        private readonly IDbExtendedService _service;

        public TestingController(IDbExtendedService service)
        {
            _service= service;
        }
        [HttpPost]
        [Route("Clear")]
        public IActionResult Clear()
        {
            _service.DeleteAll();
            return Ok();
        }
    }
}