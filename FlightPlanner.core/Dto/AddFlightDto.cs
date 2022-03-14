namespace FlightPlanner.core.Dto
{
    public class AddFlightDto
    {
        public int Id { get; set; }
        public AddAirportDto From { get; set; }
        public AddAirportDto To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}