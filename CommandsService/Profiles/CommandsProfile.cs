using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;
using PlatformsService;

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
                    mce => mce.MapFrom(
                        platformPublishedDTO => platformPublishedDTO.Id)
                    )
                .ForMember(
                    platform => platform.Id,
                    mce => mce.Ignore() // ignore, so platformPublishedDTO.Id isn't mapped to platform.Id
                );
            CreateMap<Platform, GenericEventDTO>();
            CreateMap<GRPCPlatformModel, Platform>()
                .ForMember(
                    platform => platform.ExternalId,
                    mce => mce.MapFrom(grpcPlatformModel => grpcPlatformModel.PlatformId
                    )
                );
        }
    }
}
