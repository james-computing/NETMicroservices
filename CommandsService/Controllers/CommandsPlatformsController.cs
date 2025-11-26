using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsPlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsPlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetAllPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Commands Service");
            IEnumerable<Platform> platforms = _repo.GetAllPlatforms();
            IEnumerable<PlatformReadDTO> platformsDTOs = _mapper.Map<IEnumerable<PlatformReadDTO>>(platforms);
            return Ok(platformsDTOs);
        }

        // Only for testing a POST request from the Platforms service to the Commands service
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            const string warningMessage = "The data sent synchronously to the Commands Service by POST request isn't used. It only uses the data sent asynchronously by the message bus.";
            Console.WriteLine("--> Inbound POST # Command Service");
            Console.WriteLine(warningMessage);
            return Ok($"Inbound test from Platforms Controller\n{warningMessage}");
        }
    }
}
