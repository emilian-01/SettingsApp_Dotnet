// Services/IAuthService.cs
using SettingsApp.Client.Models;
using SettingsAPI.Models.DTOs;

namespace SettingsApp.Client.Services
{
    public interface IAuthService
    {
        Task<UserDto?> Login(UserLoginDto userLogin);
        Task<UserDto?> Register(UserRegisterDto userRegister);
        Task<UserDto?> CreateUser(AdminCreateUserDto userCreate);

        Task Logout();
        Task<bool> IsLoggedIn();
        Task<string> GetUserRole();
    }
}