using System;
using System.Collections.Generic;
using System.Linq;
using FlightPlaner.Exception;
using FlightPlaner.Models;

namespace FlightPlaner.Archive
{
    public static class FlightStorage
    {  
        private static readonly object requestLock = new object();
        private static List<Flight> _flights = new List<Flight>();
        private static int _id;
        

        public static Flight AddFlight(AddFlightRequest request)
        {
            var flight = new Flight
            {
                From = request.From,
                To = request.To,
                ArrivalTime = request.ArrivalTime,
                DepartureTime = request.DepartureTime,
                Carrier = request.Carrier,
                Id = ++_id
            };

            if (ChekNullOrEmpty(flight))
            {
                throw new NullOrEmptyException();
            }

            if (CheckDuplicateFlight(flight))
            {
                throw new DuplicateDataException();
            }

            if (CheckDuplicateAirport(flight))
            {
                throw new DuplicateAirportException();
            }

            if (CheckDateTime(flight))
            {
                throw new DateTimeException();
            }

            _flights.Add(flight);
            return flight;
        }
        public static Flight GetFlight(int id)
        {
            lock (requestLock)
            {
                var flight = _flights.Find(x => x.Id == id);
                if (flight == null)
                {
                    throw new InvalidIdException();
                }

                return _flights.FirstOrDefault(s => s.Id == id);
            }
        }
        public static List<Airport> searchAirports(string phrase)
        {
            phrase = phrase.ToLower().Trim();
            var resultFrom = _flights.Where(a =>
                    a.From.AirportName.ToLower().Trim().Contains(phrase) ||
                    a.From.Country.ToLower().Trim().Contains(phrase) ||
                    a.From.City.ToLower().Trim().Contains(phrase))
                .Select(a => a.From).ToList();

            var resultTo = _flights.Where(a =>
                    a.To.AirportName.ToLower().Trim().Contains(phrase) ||
                    a.To.Country.ToLower().Trim().Contains(phrase) ||
                    a.To.City.ToLower().Trim().Contains(phrase))
                .Select(a => a.To).ToList();

            return resultFrom.Concat(resultTo).ToList();
        }
        public static PageResult SearchFlight(SearchFlightsRequest request)
        {
            var pageResult = new PageResult
            {
                Items = new List<Flight>()
            };

            var flights = _flights.Where(x =>
                    x.From.AirportName.ToLower().Trim().Contains(request.From.ToLower().Trim()) ||
                    x.To.AirportName.ToLower().Trim().Contains(request.To.ToLower().Trim()) ||
                    DateTime.Parse(x.DepartureTime) == request.DepartureDate)
                .Select(x => x).ToList();

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
        public static void ClearFlights()
        {
            _flights.Clear();
            _id = 0;
        }
        public static void DeleteFlights(int id)
        {
            lock (requestLock)
            {
                var flightId = _flights.Find(s => s.Id == id);
                if (flightId != null)
                {
                    _flights.Remove(flightId);
                }
            }
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
        private static bool CheckDuplicateFlight(Flight flight)
        {
            lock (requestLock)
            {
                if (_flights.ToList().Any(fly => fly.To.AirportName == flight.To.AirportName &&
                                                 fly.To.City == flight.To.City &&
                                                 fly.To.Country == flight.To.Country &&
                                                 fly.From.City == flight.From.City &&
                                                 fly.From.Country == flight.From.Country &&
                                                 fly.From.AirportName == flight.From.AirportName &&
                                                 fly.ArrivalTime == flight.ArrivalTime &&
                                                 fly.Carrier == flight.Carrier &&
                                                 fly.DepartureTime == flight.DepartureTime))
                {
                    throw new DuplicateDataException();
                }

                return false;
            }
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