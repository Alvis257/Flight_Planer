using FlightPlanner.core.Dto;

namespace FlightPlanner.core.Services
{
    public interface IValidator
    {
        bool IsValid(AddFlightDto request);
    }
}
