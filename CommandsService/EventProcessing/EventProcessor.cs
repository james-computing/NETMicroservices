using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    enum EventType
    {
        PlatformPublished,
        Undetermined,
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            EventType eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                case EventType.Undetermined:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining event");

            GenericEventDTO? genericEventDTO = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);
            
            if (genericEventDTO == null)
            {
                Console.WriteLine("Failed to deserialize notificationMessage in EventProcessor.DetermineEvent");
                return EventType.Undetermined;
            }

            switch (genericEventDTO.Event)
            {
                case ("PlatformPublished"):
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                ICommandRepo repo = scope.ServiceProvider.GetService<ICommandRepo>()!;
                PlatformPublishedDTO? platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);

                if (platformPublishedDTO == null)
                {
                    Console.WriteLine("Failed to deserialize platformPublishedMessage in EventProcessor.AddPlatform");
                    return;
                }

                try
                {
                    Platform platform = _mapper.Map<Platform>(platformPublishedDTO);
                    if (!repo.ExternalPlatformExists(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists...");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not add platform to database: {ex.Message}");
                }
            }
        }
    }
}
