using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Models;
using PlatformsService; // for PlatformResponse and etc
using static PlatformsService.GRPCPlatform;

namespace PlatformService.DataServices.SyncDataServices.Grpc
{
    public class GrpcPlatformsService : GRPCPlatformBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformsService(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            PlatformResponse response = new PlatformResponse();
            IEnumerable<Platform> platforms = _repo.GetAllPlatforms();
            foreach(Platform platform in platforms)
            {
                response.Platform.Add(_mapper.Map<GRPCPlatformModel>(platform));
            }
            return response;
        }
    }
}
