// SettingsAPI.Repositories/IUserRepository.cs
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}