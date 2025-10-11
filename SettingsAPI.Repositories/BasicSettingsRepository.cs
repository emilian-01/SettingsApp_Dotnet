
// SettingsAPI.Repositories/BasicSettingsRepository.cs
using Microsoft.EntityFrameworkCore;
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public class BasicSettingsRepository : IBasicSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public BasicSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BasicSettings?> GetBasicSettingsByUserIdAsync(int userId)
        {
            return await _context.BasicSettings
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<BasicSettings> CreateBasicSettingsAsync(BasicSettings settings)
        {
            await _context.BasicSettings.AddAsync(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<BasicSettings?> UpdateBasicSettingsAsync(BasicSettings settings)
        {
            var existingSettings = await _context.BasicSettings
                .FirstOrDefaultAsync(s => s.Id == settings.Id);

            if (existingSettings == null)
                return null;

            // Update properties
            existingSettings.EnableNotifications = settings.EnableNotifications;
            existingSettings.DarkModeEnabled = settings.DarkModeEnabled;
            existingSettings.AutoSaveEnabled = settings.AutoSaveEnabled;
            existingSettings.SoundEnabled = settings.SoundEnabled;

            await _context.SaveChangesAsync();
            return existingSettings;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}