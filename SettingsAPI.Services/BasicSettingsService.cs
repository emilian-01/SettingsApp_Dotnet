// SettingsAPI.Services/BasicSettingsService.cs
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Repositories;

namespace SettingsAPI.Services
{
    public class BasicSettingsService : IBasicSettingsService
    {
        private readonly IBasicSettingsRepository _repository;

        public BasicSettingsService(IBasicSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<BasicSettingsDto?> GetBasicSettingsAsync(int userId)
        {
            var settings = await _repository.GetBasicSettingsByUserIdAsync(userId);
            
            if (settings == null)
                return null;

            return new BasicSettingsDto
            {
                EnableNotifications = settings.EnableNotifications,
                DarkModeEnabled = settings.DarkModeEnabled,
                AutoSaveEnabled = settings.AutoSaveEnabled,
                SoundEnabled = settings.SoundEnabled
            };
        }

        public async Task<BasicSettingsDto?> UpdateBasicSettingsAsync(int userId, BasicSettingsDto settingsDto)
        {
            var existingSettings = await _repository.GetBasicSettingsByUserIdAsync(userId);
            
            if (existingSettings == null)
                return null;

            existingSettings.EnableNotifications = settingsDto.EnableNotifications;
            existingSettings.DarkModeEnabled = settingsDto.DarkModeEnabled;
            existingSettings.AutoSaveEnabled = settingsDto.AutoSaveEnabled;
            existingSettings.SoundEnabled = settingsDto.SoundEnabled;

            var updatedSettings = await _repository.UpdateBasicSettingsAsync(existingSettings);
            
            if (updatedSettings == null)
                return null;

            return new BasicSettingsDto
            {
                EnableNotifications = updatedSettings.EnableNotifications,
                DarkModeEnabled = updatedSettings.DarkModeEnabled,
                AutoSaveEnabled = updatedSettings.AutoSaveEnabled,
                SoundEnabled = updatedSettings.SoundEnabled
            };
        }
    }
}