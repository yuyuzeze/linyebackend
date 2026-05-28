using Api.Models.Dtos;
using AutoMapper;
using Infrastructure.Entities;

namespace Api.Models.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<DemoItem, DemoItemDto>();
        CreateMap<CreateDemoItemDto, DemoItem>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());
        CreateMap<UpdateDemoItemDto, DemoItem>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());
    }
}
