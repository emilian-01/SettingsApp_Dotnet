using SettingsApp.Client.Models;

namespace SettingsApp.Client.Services
{
    public interface IUsersService
    {
        Task<List<User>> GetUsers();
        Task UpdateUser(User user);
    }
}
