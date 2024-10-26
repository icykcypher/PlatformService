using PlatformService.Models;

namespace PlatformService.Repositories
{
    public interface IPlatformRepository
    {
        Task AddPlatform(Platform platform);
        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task<Platform> GetPlatformById(int id);
        bool SaveChanges();
    }
}