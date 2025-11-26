using PlatformService.DTOs;

namespace PlatformService.DataServices.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDTO platformReadDTO);
    }
}
