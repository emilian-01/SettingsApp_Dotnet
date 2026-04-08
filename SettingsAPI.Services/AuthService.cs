
// SettingsAPI.Services/AuthService.cs
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SettingsAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBasicSettingsRepository _basicSettingsRepository;
        private readonly IAdvancedSettingsRepository _advancedSettingsRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IBasicSettingsRepository basicSettingsRepository,
            IAdvancedSettingsRepository advancedSettingsRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _basicSettingsRepository = basicSettingsRepository;
            _advancedSettingsRepository = advancedSettingsRepository;
            _configuration = configuration;
        }

        public async Task<UserDto?> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            if (await UserExistsAsync(userRegisterDto.Username))
                return null;

            CreatePasswordHash(userRegisterDto.Password, 
                out byte[] passwordHash, 
                out byte[] passwordSalt);

            var user = new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User" // Default role
            };

            await _userRepository.CreateUserAsync(user);

            // Create default basic settings for the user
            var basicSettings = new BasicSettings
            {
                UserId = user.Id,
                EnableNotifications = true,
                DarkModeEnabled = false,
                AutoSaveEnabled = true,
                SoundEnabled = true
            };
            await _basicSettingsRepository.CreateBasicSettingsAsync(basicSettings);

            // Create default advanced settings for the user
            var advancedSettings = new AdvancedSettings
            {
                UserId = user.Id,
                DebugModeEnabled = false,
                DetailedLoggingEnabled = false,
                BetaFeaturesEnabled = false,
                PerformanceMode = false,
                RemoteAccessEnabled = false
            };
            await _advancedSettingsRepository.CreateAdvancedSettingsAsync(advancedSettings);

            // Return user DTO with token
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateToken(user)
            };
        }

        public async Task<UserDto?> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userLoginDto.Username);
            
            if (user == null)
                return null;

            if (!VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateToken(user)
            };
        }

        public async Task<UserDto?> CreateUserAsync(AdminCreateUserDto userCreateDto)
        {
            CreatePasswordHash(userCreateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = userCreateDto.IsAdmin ? "Admin" : "User"
            };

            await _userRepository.CreateUserAsync(user);

            // Create default settings for the new user
            var basicSettings = new BasicSettings { UserId = user.Id };
            await _basicSettingsRepository.CreateBasicSettingsAsync(basicSettings);

            var advancedSettings = new AdvancedSettings { UserId = user.Id };
            await _advancedSettingsRepository.CreateAdvancedSettingsAsync(advancedSettings);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = GenerateToken(user)
            };
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username) != null;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
