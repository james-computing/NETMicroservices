using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<Command, CommandReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<PlatformPublishedDTO, Platform>()
                .ForMember(
                    platform => platform.ExternalId,
                    memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                        platformPublishedDTO => platformPublishedDTO.Id)
                    );
            CreateMap<Platform, GenericEventDTO>();
        }
    }
}
