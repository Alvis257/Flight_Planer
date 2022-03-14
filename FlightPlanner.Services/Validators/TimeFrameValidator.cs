using System;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Services;

namespace FlightPlanner.Services.Validators
{
    public class TimeFrameValidator:IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return DateTime.Parse(request.ArrivalTime) > DateTime.Parse(request.DepartureTime);
        }
    }
}
