using System.Collections.Generic;
using FlightPlanner.core.Models;

namespace FlightPlanner.core.Dto
{
    public class PageResult
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public List<Flight> Items { get; set; }
    }
}