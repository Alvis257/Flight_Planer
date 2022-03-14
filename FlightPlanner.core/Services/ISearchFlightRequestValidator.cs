using FlightPlanner.core.Dto;

namespace FlightPlanner.core.Services
{
    public interface ISearchFlightRequestValidator
    {
        bool IsValid(SearchFlightsRequest request);
    }
}
