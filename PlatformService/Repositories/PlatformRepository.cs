using PlatformService.Data;
using PlatformService.Models;
using Microsoft.EntityFrameworkCore;

namespace PlatformService.Repositories
{
    public class PlatformRepository(AppDbContext dbContext) : IPlatformRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public bool SaveChanges() => _dbContext.SaveChanges() >= 0;

        public async Task<IEnumerable<Platform>> GetAllPlatforms() => await _dbContext.Platforms.ToListAsync();

        public async Task<Platform> GetPlatformById(int id)
            => await _dbContext.Platforms.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NullReferenceException();

        public async Task AddPlatform(Platform platform)
        {
            ArgumentNullException.ThrowIfNull(platform);

            await _dbContext.Platforms.AddAsync(platform);
            await _dbContext.SaveChangesAsync();
        }
    }
}