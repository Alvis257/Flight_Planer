using System;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FromAirportCityValidator:IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return !string.IsNullOrEmpty(request?.From?.City);
        }
    }
}
