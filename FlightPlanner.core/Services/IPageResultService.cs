using FlightPlanner.core.Dto;
using FlightPlanner.core.Models;

namespace FlightPlanner.core.Services
{
    public interface IPageResultService:IEntityService<Flight>
    {
        PageResult SearchFlights(SearchFlightsRequest request);
    }
}
