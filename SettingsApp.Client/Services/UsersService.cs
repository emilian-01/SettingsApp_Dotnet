using System.Net.Http.Json;
using SettingsApp.Client.Models;

namespace SettingsApp.Client.Services
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;

        public UsersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>("api/Auth/users");
        }

        public async Task UpdateUser(User user)
        {
            await _httpClient.PutAsJsonAsync($"api/Auth/users/{user.Id}", user);
        }
    }
}
