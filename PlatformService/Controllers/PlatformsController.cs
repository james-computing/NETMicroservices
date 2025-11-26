using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DataServices.AsyncDataServices;
using PlatformService.DataServices.SyncDataServices.Http;
using PlatformService.DTOs;
using PlatformService.Models;
using System.Collections.Specialized;

namespace PlatformService.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("--> Get platforms...");
            IEnumerable<Platform> platforms = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO?> GetPlatformById(int id)
        {
            Platform? platform = _repository.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platform));
            }

        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatformAsync(PlatformCreateDTO platformCreateDTO)
        {
            Platform platform = _mapper.Map<Platform>(platformCreateDTO);
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            // Tell the Commands service about the platform created

            PlatformReadDTO platformReadDTO = _mapper.Map<PlatformReadDTO>(platform);

            // Send sync message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            // Send async message
            try
            {
                PlatformPublishedDTO platformPublishedDTO = _mapper.Map<PlatformPublishedDTO>(platformReadDTO);
                platformPublishedDTO.Event = "PlatformPublished";
                await _messageBusClient.PublishNewPlatform(platformPublishedDTO);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDTO.Id}, platformReadDTO);
        }
    }
}
