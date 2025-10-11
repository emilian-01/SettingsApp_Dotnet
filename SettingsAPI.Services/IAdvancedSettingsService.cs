

// SettingsAPI.Services/IAdvancedSettingsService.cs
using SettingsAPI.Models.DTOs;

namespace SettingsAPI.Services
{
    public interface IAdvancedSettingsService
    {
        Task<AdvancedSettingsDto?> GetAdvancedSettingsAsync(int userId);
        Task<AdvancedSettingsDto?> UpdateAdvancedSettingsAsync(int userId, AdvancedSettingsDto settingsDto);
    }
}