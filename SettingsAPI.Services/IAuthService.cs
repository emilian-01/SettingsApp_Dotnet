// SettingsAPI.Services/IAuthService.cs
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;

namespace SettingsAPI.Services
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<UserDto?> LoginAsync(UserLoginDto userLoginDto);
        Task<UserDto?> CreateUserAsync(AdminCreateUserDto userCreateDto);

        Task<bool> UserExistsAsync(string username);
        string GenerateToken(User user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
