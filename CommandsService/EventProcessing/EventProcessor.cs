using AutoMapper;
using System.Text.Json;
using CommandsService.Dtos;
using CommandsService.Data;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper) : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly IMapper _mapper = mapper;

        public void ProcessEvent(string message)
        {
            var @event = DetermineEvent(message);

            switch (@event)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                case EventType.Undetermined:
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            Console.WriteLine("--> Determining event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message) ?? throw new Exception("--> Null from generic event type");

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine Event Type");
                    return EventType.Undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);

                    if (!repo.ExternalPlatformExists(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Succesfully added Platform!");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists...");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not add Platform to DB: {e.Message}");
                    throw;
                }
            }
        }
    }

    internal enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}