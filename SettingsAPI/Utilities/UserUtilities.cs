// SettingsAPI/Utilities/UserUtilities.cs
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SettingsAPI.Utilities
{
    public static class UserUtilities
    {
        public static int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }
    }
}