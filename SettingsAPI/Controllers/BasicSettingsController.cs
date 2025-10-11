// SettingsAPI/Controllers/BasicSettingsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Services;
using SettingsAPI.Utilities;

namespace SettingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires authentication
    public class BasicSettingsController : ControllerBase
    {
        private readonly IBasicSettingsService _settingsService;

        public BasicSettingsController(IBasicSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ActionResult<BasicSettingsDto>> GetBasicSettings()
        {
            var userId = UserUtilities.GetUserIdFromClaims(User);

            if (userId == 0)
                return Unauthorized();

            var settings = await _settingsService.GetBasicSettingsAsync(userId);

            if (settings == null)
                return NotFound("Basic settings not found");

            return Ok(settings);
        }

        [HttpPut]
        public async Task<ActionResult<BasicSettingsDto>> UpdateBasicSettings(BasicSettingsDto settingsDto)
        {
            var userId = UserUtilities.GetUserIdFromClaims(User);

            if (userId == 0)
                return Unauthorized();

            var updatedSettings = await _settingsService.UpdateBasicSettingsAsync(userId, settingsDto);

            if (updatedSettings == null)
                return NotFound("Basic settings not found");

            return Ok(updatedSettings);
        }
    }
}