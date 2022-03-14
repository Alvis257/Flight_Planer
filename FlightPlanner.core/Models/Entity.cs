using FlightPlanner.core.Interfaces;

namespace FlightPlanner.core.Models
{
    public abstract class Entity :IEntity
    {
        public int Id { get; set; }
    }
}
