using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Models;
using FlightPlanner.core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlaner.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors]
    public class CustomerController : ControllerBase
    {
        private readonly IPageResultService _pageService;
        private readonly IFlightService _flightService;
        private readonly IEnumerable<ISearchFlightRequestValidator> _validators;
        private IMapper _mapper;
        public CustomerController(IPageResultService pageService,
            IEnumerable<ISearchFlightRequestValidator> validators,
            IMapper mapper, IFlightService flightService)
        {
            _flightService = flightService;
            _pageService = pageService;
            _validators = validators;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
           List<Airport> flight = _flightService.SearchAirports(search);
           var result = _mapper.Map<List<AddAirportDto>>(flight);
           return Ok(result);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest req)
        {
            if (!_validators.All(v=>v.IsValid(req)))
                return BadRequest();

            PageResult flight = _pageService.SearchFlights(req);
            return Ok(flight);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlight(int id)
        {
            var flight = _flightService.GetFlightWithAirports(id);

            return flight == null ? NotFound() : (IActionResult)Ok(_mapper.Map<AddFlightDto>(flight));
        }
    }
}
