using System;

namespace FlightPlanner.core.Dto
{
    public class SearchFlightsRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}