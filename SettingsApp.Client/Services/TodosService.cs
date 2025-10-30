// Services/SettingsService.cs
using System.Net.Http.Json;
using SettingsAPI.Models;
using SettingsApp.Client.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace SettingsApp.Client.Services
{
    public class TodosService : ITodosService
    {
        private readonly HttpClient _httpClient;

        public TodosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<Todo> GetTodos()
        {
            throw new NotImplementedException();
        }

    public async Task<Result<PagedResponse<TodoDto>>> GetTodos(int pageNumber = 1, int pageSize = 20, string? searchFilter = null)
    {
        try
        {
            var query = new Dictionary<string, string?>()
            {
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"]   = pageSize.ToString(),
                ["searchFilter"] = string.IsNullOrWhiteSpace(searchFilter) ? null : searchFilter
            };

            var url = QueryHelpers.AddQueryString("api/Todos", query!);
            var page = await _httpClient.GetFromJsonAsync<PagedResponse<TodoDto>>(url);

            if (page is null)
                return Result<PagedResponse<TodoDto>>.Failure("The server returned no content.");

            return Result<PagedResponse<TodoDto>>.Success(page);
        }
        catch (Exception ex)
        {
            return Result<PagedResponse<TodoDto>>.Failure($"Error fetching objects: {ex.Message}");
        }
    }

        public Task<TodoDto> CreateTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> DeleteTodos(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Todos/{id}");
                if (!response.IsSuccessStatusCode)
                    return Result<bool>.Failure("Failed to delete object");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting object: {ex.Message}");
            }
        }


        // public async Task<BasicSettings> GetBasicSettings()
        // {
        //     var response = await _httpClient.GetFromJsonAsync<BasicSettings>("api/BasicSettings");
        //     return response ?? new BasicSettings();
        // }

        // public async Task<AdvancedSettings> GetAdvancedSettings()
        // {
        //     var response = await _httpClient.GetFromJsonAsync<AdvancedSettings>("api/AdvancedSettings");
        //     return response ?? new AdvancedSettings();
        // }

        // public async Task<BasicSettings> UpdateBasicSetting(BasicSettings setting)
        // {
        //     var response = await _httpClient.PutAsJsonAsync($"api/BasicSettings/{setting.Id}", setting);
        //     return await response.Content.ReadFromJsonAsync<BasicSettings>() ?? setting;
        // }

        // public async Task<AdvancedSettings> UpdateAdvancedSetting(AdvancedSettings setting)
        // {
        //     var response = await _httpClient.PutAsJsonAsync($"api/AdvancedSettings/{setting.Id}", setting);
        //     return await response.Content.ReadFromJsonAsync<AdvancedSettings>() ?? setting;
        // }
    }
}