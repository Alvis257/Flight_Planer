﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Services;

namespace FlightPlanner.Services.Validators
{
    public class SearchFlightRequestValidator:ISearchFlightRequestValidator
    {
        public bool IsValid(SearchFlightsRequest request)
        {
            return request.From.ToLower() != request.To.ToLower();
        }
    }
}
