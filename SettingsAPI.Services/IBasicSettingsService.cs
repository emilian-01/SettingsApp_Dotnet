
// SettingsAPI.Services/IBasicSettingsService.cs
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;

namespace SettingsAPI.Services
{
    public interface IBasicSettingsService
    {
        Task<BasicSettingsDto?> GetBasicSettingsAsync(int userId);
        Task<BasicSettingsDto?> UpdateBasicSettingsAsync(int userId, BasicSettingsDto settingsDto);
    }
}