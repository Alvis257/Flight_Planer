using AutoMapper;
using FlightPlanner.core.Dto;
using FlightPlanner.core.Models;

namespace FlightPlanner.Services.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Flight, AddFlightDto>();
                cfg.CreateMap<AddFlightDto, Flight>();
                cfg.CreateMap<Airport, AddAirportDto>()
                    .ForMember(
                        dest => dest.Airport, 
                        opt => 
                        opt.MapFrom(d => d.AirportName));
                cfg.CreateMap<AddAirportDto, Airport>()
                    .ForMember(
                        dest => dest.AirportName,
                        opt => 
                        opt.MapFrom(d => d.Airport))
                    .ForMember(
                        dest => dest.Id,
                        obt => 
                        obt.Ignore());
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            // use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
            var mapper = configuration.CreateMapper();
            return mapper;
        }
    }
}
