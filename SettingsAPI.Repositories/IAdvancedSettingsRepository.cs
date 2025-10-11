// SettingsAPI.Repositories/IAdvancedSettingsRepository.cs
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public interface IAdvancedSettingsRepository
    {
        Task<AdvancedSettings?> GetAdvancedSettingsByUserIdAsync(int userId);
        Task<AdvancedSettings> CreateAdvancedSettingsAsync(AdvancedSettings settings);
        Task<AdvancedSettings?> UpdateAdvancedSettingsAsync(AdvancedSettings settings);
        Task<bool> SaveChangesAsync();
    }
}