using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("/api/[controller]/platformId/{platformId}")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform for platformId = {platformId}");
            if(_repo.PlatformExists(platformId) == false)
            {
                return NotFound();
            }

            IEnumerable<Command> commands = _repo.GetCommandsForPlatform(platformId);
            IEnumerable<CommandReadDTO> commandsDTOs = _mapper.Map<IEnumerable<CommandReadDTO>>(commands);
            return Ok(commandsDTOs);
        }

        [HttpGet("/commandId/{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDTO> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform for platformId = {platformId}, commandId = {commandId}");
            if (_repo.PlatformExists(platformId) == false)
            {
                return NotFound();
            }

            Command? command = _repo.GetCommand(platformId, commandId);
            if(command == null)
            {
                return NotFound();
            }

            CommandReadDTO commandReadDTO = _mapper.Map<CommandReadDTO>(command);
            return Ok(commandReadDTO);
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommandForPlatform(int platformId, CommandCreateDTO commandCreateDTO)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform for platformId = {platformId}");
            if (_repo.PlatformExists(platformId) == false)
            {
                return NotFound();
            }

            Command command = _mapper.Map<Command>(commandCreateDTO);
            _repo.CreateCommand(platformId, command);
            _repo.SaveChanges();

            CommandReadDTO commandReadDTO = _mapper.Map<CommandReadDTO>(command);
            return CreatedAtRoute(
                nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDTO.Id},
                commandReadDTO);
        }
    }
}
