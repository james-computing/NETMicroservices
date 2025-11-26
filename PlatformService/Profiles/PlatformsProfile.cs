using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformsService; // for GRPCPlatformModel

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<PlatformReadDTO, PlatformPublishedDTO>();
            CreateMap<Platform, GRPCPlatformModel>()
                .ForMember(
                    grpcPlatformModel => grpcPlatformModel.PlatformId,
                    mce => mce.MapFrom(platform => platform.Id)
                    );
        }
    }
}
