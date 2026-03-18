using AutoMapper;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Domain.Entities;

namespace TransformerMonitor.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transformer, TransformerDto>()
            .ForMember(dest => dest.LastReadings, opt => opt.MapFrom(src => src.VoltageReadings));

        CreateMap<VoltageReading, VoltageReadingDto>();

        CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.TransformerName, opt => opt.MapFrom(src => src.Transformer != null ? src.Transformer.Name : "Asset #" + src.TransformerId))
            .ForMember(dest => dest.AssignedTeamName, opt => opt.MapFrom(src => src.AssignedTeam != null ? src.AssignedTeam.Name : string.Empty))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));

        CreateMap<Team, TeamDto>()
            .ForMember(dest => dest.Technicians, opt => opt.MapFrom(src => src.Technicians.Select(t => t.Name)));
    }
}
