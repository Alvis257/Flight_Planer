using System.Threading.Tasks;
using FlightPlanner.core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlightPlanner.Data
{
    public interface IFlightPlannerDbContext
    {
        DbSet<T> Set<T>() where T: class;
        EntityEntry<T> Entry<T>(T entity) where T:class ;
        int SaveChanges();
        DbSet<Flight> Flights { get; set; }
        DbSet<Airport> Airports { get; set; }
        Task<int> SaveChangesAsync();
    }
}
