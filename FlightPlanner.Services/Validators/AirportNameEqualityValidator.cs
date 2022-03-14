using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Services;

namespace FlightPlanner.Services.Validators
{
    public class AirportNameEqualityValidator:IValidator
    {
        public bool IsValid(AddFlightDto request)
        {
            return !string.Equals(request.From.Airport.Trim(), request.To.Airport.Trim(),
                StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
