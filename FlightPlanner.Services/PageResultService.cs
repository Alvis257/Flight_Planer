using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Models;
using FlightPlanner.core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class PageResultService:EntityService<Flight>,IPageResultService
    {
        private PageResult _pageResult = new PageResult();
        public PageResultService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public PageResult SearchFlights(SearchFlightsRequest request)
        {
            _pageResult.Items = new List<Flight>();
            var flights = _dbStorageContext.Flights
                .Include(f => f.To)
                .Include(f => f.From)
                .Where(x =>
                    x.From.AirportName.ToLower().Trim().Contains(request.From.ToLower().Trim()) ||
                    x.From.AirportName.ToLower().Trim().Contains(request.To.ToLower().Trim()) ||
                    x.DepartureTime == Convert.ToString(request.DepartureDate))
                .Select(x => x)
                .ToList();

            foreach (var flight in flights)
            {
                if (flight == null)
                {
                    return _pageResult;
                }

                _pageResult.Items.Add(flight);
            }

            _pageResult.TotalItems = _pageResult.Items.Count;
            _pageResult.Page = _pageResult.TotalItems;
            return _pageResult;
        }
    }
}
