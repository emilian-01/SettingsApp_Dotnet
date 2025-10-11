

// SettingsAPI.Services/AdvancedSettingsService.cs
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Repositories;

namespace SettingsAPI.Services
{
    public class AdvancedSettingsService : IAdvancedSettingsService
    {
        private readonly IAdvancedSettingsRepository _repository;

        public AdvancedSettingsService(IAdvancedSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdvancedSettingsDto?> GetAdvancedSettingsAsync(int userId)
        {
            var settings = await _repository.GetAdvancedSettingsByUserIdAsync(userId);
            
            if (settings == null)
                return null;

            return new AdvancedSettingsDto
            {
                DebugModeEnabled = settings.DebugModeEnabled,
                DetailedLoggingEnabled = settings.DetailedLoggingEnabled,
                BetaFeaturesEnabled = settings.BetaFeaturesEnabled,
                PerformanceMode = settings.PerformanceMode,
                RemoteAccessEnabled = settings.RemoteAccessEnabled
            };
        }

        public async Task<AdvancedSettingsDto?> UpdateAdvancedSettingsAsync(int userId, AdvancedSettingsDto settingsDto)
        {
            var existingSettings = await _repository.GetAdvancedSettingsByUserIdAsync(userId);
            
            if (existingSettings == null)
                return null;

            existingSettings.DebugModeEnabled = settingsDto.DebugModeEnabled;
            existingSettings.DetailedLoggingEnabled = settingsDto.DetailedLoggingEnabled;
            existingSettings.BetaFeaturesEnabled = settingsDto.BetaFeaturesEnabled;
            existingSettings.PerformanceMode = settingsDto.PerformanceMode;
            existingSettings.RemoteAccessEnabled = settingsDto.RemoteAccessEnabled;

            var updatedSettings = await _repository.UpdateAdvancedSettingsAsync(existingSettings);
            
            if (updatedSettings == null)
                return null;

            return new AdvancedSettingsDto
            {
                DebugModeEnabled = updatedSettings.DebugModeEnabled,
                DetailedLoggingEnabled = updatedSettings.DetailedLoggingEnabled,
                BetaFeaturesEnabled = updatedSettings.BetaFeaturesEnabled,
                PerformanceMode = updatedSettings.PerformanceMode,
                RemoteAccessEnabled = updatedSettings.RemoteAccessEnabled
            };
        }
    }
}