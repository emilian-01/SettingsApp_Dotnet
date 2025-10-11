
// SettingsAPI.Repositories/IBasicSettingsRepository.cs
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public interface IBasicSettingsRepository
    {
        Task<BasicSettings?> GetBasicSettingsByUserIdAsync(int userId);
        Task<BasicSettings> CreateBasicSettingsAsync(BasicSettings settings);
        Task<BasicSettings?> UpdateBasicSettingsAsync(BasicSettings settings);
        Task<bool> SaveChangesAsync();
    }
}