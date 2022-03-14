using FlightPlanner.core.Dto;
using FlightPlanner.core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FromAirportValidator:IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return request?.From != null;
        }
    }
}
