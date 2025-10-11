// SettingsAPI/Controllers/AdvancedSettingsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Services;
using SettingsAPI.Utilities;

namespace SettingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only admin can access
    public class AdvancedSettingsController : ControllerBase
    {
        private readonly IAdvancedSettingsService _settingsService;

        public AdvancedSettingsController(IAdvancedSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ActionResult<AdvancedSettingsDto>> GetAdvancedSettings()
        {
            var userId = UserUtilities.GetUserIdFromClaims(User);

            if (userId == 0)
                return Unauthorized();

            if (!UserUtilities.IsAdmin(User))
                return Forbid("Admin role required");

            var settings = await _settingsService.GetAdvancedSettingsAsync(userId);

            if (settings == null)
                return NotFound("Advanced settings not found");

            return Ok(settings);
        }

        [HttpPut]
        public async Task<ActionResult<AdvancedSettingsDto>> UpdateAdvancedSettings(AdvancedSettingsDto settingsDto)
        {
            var userId = UserUtilities.GetUserIdFromClaims(User);

            if (userId == 0)
                return Unauthorized();

            if (!UserUtilities.IsAdmin(User))
                return Forbid("Admin role required");

            var updatedSettings = await _settingsService.UpdateAdvancedSettingsAsync(userId, settingsDto);

            if (updatedSettings == null)
                return NotFound("Advanced settings not found");

            return Ok(updatedSettings);
        }
    }
}