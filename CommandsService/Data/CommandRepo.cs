using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _dbContext;

        public CommandRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _dbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            Console.WriteLine("Ids of Platform to be added to Commands service database:");
            Console.WriteLine($"Id = {platform.Id}");
            Console.WriteLine($"ExternalId = {platform.ExternalId}");
            _dbContext.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public Command? GetCommand(int platformId, int commandId)
        {
            return _dbContext.Commands.Where(command => command.PlatformId == platformId && command.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _dbContext.Commands
                .Where(command => command.PlatformId == platformId)
                .OrderBy(command => command.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(platform => platform.Id == platformId);
        }

        public bool ExternalPlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(platform => platform.ExternalId == platformId);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }
    }
}
