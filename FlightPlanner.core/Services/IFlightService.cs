using System.Collections.Generic;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Models;

namespace FlightPlanner.core.Services
{
    public interface IFlightService:IEntityService<Flight>
    {
        Flight GetFlightWithAirports(int id);
        void DeleteFlightByID(int id);
        bool CheckDuplicateFlight(AddFlightDto flight);
        List<Airport> SearchAirports(string phrase);

    }
}
