using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlaner.Exception;
using FlightPlaner.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlaner.Archive
{
    public static class FlightStorage
    {
        private static readonly object requestLock = new object();
        private static int _id;

        public static Flight ConvertToFlight(AddFlightRequest request)

        {
            var flight = new Flight
            {
                From = request.From,
                To = request.To,
                ArrivalTime = request.ArrivalTime,
                DepartureTime = request.DepartureTime,
                Carrier = request.Carrier,
            };

            if (ChekNullOrEmpty(flight))
            {
                throw new NullOrEmptyException();
            }

            if (CheckDuplicateAirport(flight))
            {
                throw new DuplicateAirportException();
            }

            if (CheckDateTime(flight))
            {
                throw new DateTimeException();
            }

            return flight;
        }

        public static Flight GetFlight(FlightPlanerDbContext context, int id)
        {
            lock (requestLock)
            {
                var flight = context.Flights
                    .Include(f => f.To)
                    .Include(f => f.From)
                    .SingleOrDefault(x => x.Id == id);

                if (flight == null)
                {
                    throw new InvalidIdException();
                }

                return flight;
            }
        }

        public static List<Airport> searchAirports(FlightPlanerDbContext context, string phrase)
        {
            phrase = phrase.ToLower().Trim();
            var resultFrom = context.Flights.Where(a =>
                    a.From.AirportName.ToLower().Trim().Contains(phrase) ||
                    a.From.Country.ToLower().Trim().Contains(phrase) ||
                    a.From.City.ToLower().Trim().Contains(phrase))
                .Select(a => a.From).ToList();

            var resultTo = context.Flights.Where(a =>
                    a.To.AirportName.ToLower().Trim().Contains(phrase) ||
                    a.To.Country.ToLower().Trim().Contains(phrase) ||
                    a.To.City.ToLower().Trim().Contains(phrase))
                .Select(a => a.To).ToList();

            return resultFrom.Concat(resultTo).ToList();
        }

        public static PageResult SearchFlight(FlightPlanerDbContext context, SearchFlightsRequest request)
        {
            var pageResult = new PageResult
            {
                Items = new List<Flight>()
            };

            var flights = context.Flights
                .Include(f => f.To)
                .Include(f => f.From)
                .Where(x =>
                    x.From.AirportName.ToLower().Trim().Contains(request.From.ToLower().Trim()) ||
                    x.To.AirportName.ToLower().Trim().Contains(request.To.ToLower().Trim()) ||
                    x.DepartureTime == Convert.ToString(request.DepartureDate))
                .Select(x => x)
                .ToList();

            foreach (var flight in flights)
            {
                if (flight == null)
                {
                    return pageResult;
                }

                pageResult.Items.Add(flight);
            }

            pageResult.TotalItems = pageResult.Items.Count;
            pageResult.Page = pageResult.TotalItems;
            return pageResult;
        }


        private static bool ChekNullOrEmpty(Flight flight)
        {
            if (flight.From == null ||
                flight.To == null ||
                flight.To.AirportName == null || string.IsNullOrEmpty(flight.To.AirportName) ||
                flight.To.City == null || string.IsNullOrEmpty(flight.To.City) ||
                flight.To.Country == null || string.IsNullOrEmpty(flight.To.Country) ||
                flight.From.City == null || string.IsNullOrEmpty(flight.From.City) ||
                flight.From.AirportName == null || string.IsNullOrEmpty(flight.From.Country) ||
                flight.From.AirportName == null || string.IsNullOrEmpty(flight.From.AirportName) ||
                flight.ArrivalTime == null || string.IsNullOrEmpty(flight.ArrivalTime) ||
                flight.Carrier == null || string.IsNullOrEmpty(flight.Carrier) ||
                flight.DepartureTime == null || string.IsNullOrEmpty(flight.DepartureTime))
            {
                throw new NullOrEmptyException();
            }

            return false;
        }

        private static bool CheckDuplicateAirport(Flight flight)
        {
            if (flight.From.AirportName.ToLower() == flight.To.AirportName.ToLower() ||
                flight.From.City.ToLower() == flight.To.City.ToLower())
            {
                throw new DuplicateAirportException();
            }

            return false;
        }

        public static bool CheckInvalidAirportSearch(SearchFlightsRequest request)
        {
            return request.From == null ||
                   request.To == null ||
                   request.DepartureDate.Equals(null) ||
                   request.From.ToLower() == request.To.ToLower();
        }

        private static bool CheckDateTime(Flight flight)
        {
            if (DateTime.Parse(flight.ArrivalTime) <= DateTime.Parse(flight.DepartureTime))
            {
                throw new DateTimeException();
            }

            return false;
        }
    }
}