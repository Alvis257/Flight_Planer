using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Services;

namespace FlightPlanner.Services.Validators
{
    public class ToAirportCountryValidator:IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return !string.IsNullOrEmpty(request?.To?.Country);
        }
    }
}
