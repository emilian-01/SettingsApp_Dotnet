// // SettingsAPI/Data/AdminUserSeed.cs
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using SettingsAPI.Models;
// using SettingsAPI.Repositories;
// using SettingsAPI.Services;
// using System;

// namespace SettingsAPI.Data
// {
//     public static class AdminUserSeed
//     {
//         public static async Task SeedAdminUser(IServiceProvider serviceProvider)
//         {
//             using var scope = serviceProvider.CreateScope();
//             var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//             var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

//             // Apply migrations if they haven't been applied
//             await dbContext.Database.MigrateAsync();

//             // Check if admin user exists
//             var adminUser = await dbContext.Users
//                 .FirstOrDefaultAsync(u => u.Username == "admin");

//             if (adminUser == null)
//             {
//                 // Create admin user
//                 authService.CreatePasswordHash("Admin@123", out byte[] passwordHash, out byte[] passwordSalt);

//                 adminUser = new User
//                 {
//                     Username = "admin",
//                     Email = "admin@example.com",
//                     PasswordHash = passwordHash,
//                     PasswordSalt = passwordSalt,
//                     Role = "Admin"
//                 };

//                 await dbContext.Users.AddAsync(adminUser);
//                 await dbContext.SaveChangesAsync();

//                 // Create default settings for admin
//                 var basicSettings = new BasicSettings
//                 {
//                     UserId = adminUser.Id,
//                     EnableNotifications = true,
//                     DarkModeEnabled = true,
//                     AutoSaveEnabled = true,
//                     SoundEnabled = true
//                 };

//                 var advancedSettings = new AdvancedSettings
//                 {
//                     UserId = adminUser.Id,
//                     DebugModeEnabled = true,
//                     DetailedLoggingEnabled = true,
//                     BetaFeaturesEnabled = true,
//                     PerformanceMode = true,
//                     RemoteAccessEnabled = true
//                 };

//                 await dbContext.BasicSettings.AddAsync(basicSettings);
//                 await dbContext.AdvancedSettings.AddAsync(advancedSettings);
//                 await dbContext.SaveChangesAsync();
//             }
//         }
//     }
// }