using CommandsService.Models;

namespace CommandsService.DataServices.Sync.Grpc
{
    public interface IPlatformDataClient
    {
        public IEnumerable<Platform>? ReturnAllPlatform();
    }
}
