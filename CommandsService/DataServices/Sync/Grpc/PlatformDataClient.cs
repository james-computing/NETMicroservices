using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformsService;
using static PlatformsService.GRPCPlatform;

namespace CommandsService.DataServices.Sync.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform>? ReturnAllPlatform()
        {
            string grpcAddress = _configuration.GetValue<string>("GrpcPlatform")!;
            Console.WriteLine($"--> Calling GRPC Service {grpcAddress}");
            GrpcChannel channel = GrpcChannel.ForAddress(grpcAddress);
            GRPCPlatformClient client = new GRPCPlatformClient(channel);
            GetAllRequest request = new GetAllRequest();

            try
            {
                PlatformResponse response = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(response.Platform);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Couldn't call GRPC server: {ex.Message}");
            }
            return null;
        }
    }
}
