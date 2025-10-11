
// SettingsAPI.Repositories/AdvancedSettingsRepository.cs
using Microsoft.EntityFrameworkCore;
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public class AdvancedSettingsRepository : IAdvancedSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public AdvancedSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AdvancedSettings?> GetAdvancedSettingsByUserIdAsync(int userId)
        {
            return await _context.AdvancedSettings
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<AdvancedSettings> CreateAdvancedSettingsAsync(AdvancedSettings settings)
        {
            await _context.AdvancedSettings.AddAsync(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<AdvancedSettings?> UpdateAdvancedSettingsAsync(AdvancedSettings settings)
        {
            var existingSettings = await _context.AdvancedSettings
                .FirstOrDefaultAsync(s => s.Id == settings.Id);

            if (existingSettings == null)
                return null;

            // Update properties
            existingSettings.DebugModeEnabled = settings.DebugModeEnabled;
            existingSettings.DetailedLoggingEnabled = settings.DetailedLoggingEnabled;
            existingSettings.BetaFeaturesEnabled = settings.BetaFeaturesEnabled;
            existingSettings.PerformanceMode = settings.PerformanceMode;
            existingSettings.RemoteAccessEnabled = settings.RemoteAccessEnabled;

            await _context.SaveChangesAsync();
            return existingSettings;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}