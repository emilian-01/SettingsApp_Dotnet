// Services/SettingsService.cs
using System.Net.Http.Json;
using SettingsAPI.Models;
using SettingsApp.Client.Models;

namespace SettingsApp.Client.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly HttpClient _httpClient;

        public SettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BasicSettings> GetBasicSettings()
        {
            var response = await _httpClient.GetFromJsonAsync<BasicSettings>("api/BasicSettings");
            return response ?? new BasicSettings();
        }

        public async Task<AdvancedSettings> GetAdvancedSettings()
        {
            var response = await _httpClient.GetFromJsonAsync<AdvancedSettings>("api/AdvancedSettings");
            return response ?? new AdvancedSettings();
        }

        public async Task<BasicSettings> UpdateBasicSetting(BasicSettings setting)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/BasicSettings/{setting.Id}", setting);
            return await response.Content.ReadFromJsonAsync<BasicSettings>() ?? setting;
        }

        public async Task<AdvancedSettings> UpdateAdvancedSetting(AdvancedSettings setting)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/AdvancedSettings/{setting.Id}", setting);
            return await response.Content.ReadFromJsonAsync<AdvancedSettings>() ?? setting;
        }
    }
}