using FlightPlanner.core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class DbExtendedService:DbService,IDbExtendedService

    {
        public DbExtendedService(IFlightPlannerDbContext context) : base(context)
        {
        }
        public void DeleteAll()
        {
            _dbStorageContext.Airports.RemoveRange(_dbStorageContext.Airports);
            _dbStorageContext.Flights.RemoveRange(_dbStorageContext.Flights);
            _dbStorageContext.SaveChanges();
        }
    }
}
