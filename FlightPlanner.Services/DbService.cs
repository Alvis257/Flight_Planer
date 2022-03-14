using System.Collections.Generic;
using System.Linq;
using FlightPlanner.core.Models;
using FlightPlanner.core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class DbService:IDbService
    {
        protected readonly IFlightPlannerDbContext _dbStorageContext;

        public DbService(IFlightPlannerDbContext context)
        {
            _dbStorageContext = context;
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            return _dbStorageContext.Set<T>();
        }

        public IEnumerable<T> Get<T>() where T : Entity
        {
            return _dbStorageContext.Set<T>().ToList();
        }

        public T GetByID<T>(int id) where T : Entity
        {
            return _dbStorageContext.Set<T>().SingleOrDefault(f=>f.Id==id);
        }

        public void Create<T>(T entity) where T : Entity
        {
            _dbStorageContext.Set<T>().Add(entity);
            _dbStorageContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : Entity
        {
            _dbStorageContext.Entry<T>(entity).State = EntityState.Modified;
            _dbStorageContext.SaveChanges();
        }

        public void Delete<T>(T entity) where T : Entity
        {
            _dbStorageContext.Set<T>().Remove(entity);
            _dbStorageContext.SaveChanges();
        }
    }
}
