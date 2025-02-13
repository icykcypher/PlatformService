using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo(AppDbContext dbContext) : ICommandRepo
    {
        private readonly AppDbContext _dbContext = dbContext;

        public void CreateCommand(int platformId, Command command)
        {
            ArgumentNullException.ThrowIfNull(command);

            command.PlatformId = platformId;
            _dbContext.Commands.Add(command);

        }

        public void CreatePlatform(Platform platform)
        {
            ArgumentNullException.ThrowIfNull(platform);

            _dbContext.Platforms.Add(platform);
        }

        public bool ExternalPlatformExists(int externalPlatformId) => _dbContext.Platforms.Any(p => p.ExternalId == externalPlatformId);

        public IEnumerable<Platform> GetAllPlatforms() => _dbContext.Platforms.ToList();

        public Command? GetCommand(int platformId, int commandId) => _dbContext.Commands
            .Where(c => c.PlatformId == platformId && c.Id == commandId)
            .FirstOrDefault();

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
            => _dbContext.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);

        public bool PlatformExists(int platformId) => _dbContext.Platforms.Any(p => p.Id == platformId);

        public bool SaveChanges() => _dbContext.SaveChanges() >= 0;
    }
}