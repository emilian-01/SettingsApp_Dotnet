// Services/AuthService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using SettingsApp.Client.Models;
using SettingsAPI.Models.DTOs;

namespace SettingsApp.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<UserDto> Login(UserLoginDto userLogin)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", userLogin);
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            
            if (result != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userRole", result.Role);
            }
            
            // return result ?? new AuthResponse { Message = "An error occurred during login." };
            return result;
        }

        public async Task<UserDto> Register(UserRegisterDto userRegister)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", userRegister);
            var result = await response.Content.ReadFromJsonAsync<UserDto>();
            
            // return result ?? new UserDto { Success = false, Message = "An error occurred during registration." };
            return result;
        }

        public async Task Logout()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userRole");
        }

        public async Task<bool> IsLoggedIn()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string> GetUserRole()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userRole") ?? string.Empty;
        }
    }
}