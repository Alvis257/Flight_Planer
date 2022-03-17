using System.Collections.Generic;
using FlightPlanner.core.Models;
using FlightPlanner.core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FlightPlanner.core.Dto;

namespace FlightPlanner.Services
{
    public class FlightService:EntityService<Flight>,IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public Flight GetFlightWithAirports(int id)
        {
           return Query()
               .Include(f => f.From)
               .Include(f => f.To)
               .SingleOrDefault(f => f.Id == id);
        }

        public void DeleteFlightByID(int id)
        {
            var flight = GetFlightWithAirports(id);
            if (flight != null) Delete(flight);
        }

        public bool CheckDuplicateFlight(AddFlightDto flight)
        {
            return Query().Any(fly => fly.To.AirportName.Trim().ToLower() == flight.To.Airport.Trim().ToLower() &&
                                      fly.From.AirportName.Trim().ToLower() == flight.From.Airport.Trim().ToLower() &&
                                      fly.ArrivalTime.Trim().ToLower() == flight.ArrivalTime.Trim().ToLower() &&
                                      fly.DepartureTime.Trim().ToLower() == flight.DepartureTime.Trim().ToLower());
        }

        public List<Airport> SearchAirports(string phrase)
        {
            phrase = phrase.ToLower().Trim();
            var resultFrom = _dbStorageContext.Flights.Where(a =>
                    a.From.AirportName.ToLower().Trim().Contains(phrase) ||
                    a.From.Country.ToLower().Trim().Contains(phrase) ||
                    a.From.City.ToLower().Trim().Contains(phrase))
                .Select(a => a.From).ToList();

            return resultFrom;
        }
    }
}
