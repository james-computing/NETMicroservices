using PlatformService.DTOs;

namespace PlatformService.DataServices.AsyncDataServices
{
    public interface IMessageBusClient : IAsyncDisposable
    {
        public Task PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO);
    }
}
