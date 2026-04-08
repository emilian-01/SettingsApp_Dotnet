// SettingsAPI/Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Services;

namespace SettingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto userRegisterDto)
        {
            // Check if username already exists
            if (await _authService.UserExistsAsync(userRegisterDto.Username))
                return BadRequest("Username already exists");

            // Register user
            var result = await _authService.RegisterAsync(userRegisterDto);

            if (result == null)
                return BadRequest("Registration failed");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto)
        {
            var result = await _authService.LoginAsync(userLoginDto);

            if (result == null)
                return Unauthorized("Invalid username or password");

            return Ok(result);
        }

        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateUser(AdminCreateUserDto userCreateDto)
        {
            if (await _authService.UserExistsAsync(userCreateDto.Username))
                return BadRequest("Username already exists");

            var result = await _authService.CreateUserAsync(userCreateDto);

            if (result == null)
                return BadRequest("Failed to create user");

            return Ok(result);
        }
    }
}