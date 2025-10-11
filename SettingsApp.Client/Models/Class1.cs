// Models/UserLogin.cs
// namespace SettingsApp.Client.Models
// {
//     public class UserLogin
//     {
//         public string Email { get; set; } = string.Empty;
//         public string Password { get; set; } = string.Empty;
//     }
// }

// Models/UserRegister.cs
// namespace SettingsApp.Client.Models
// {
//     public class UserRegister
//     {
//         public string Email { get; set; } = string.Empty;
//         public string Password { get; set; } = string.Empty;
//         public string ConfirmPassword { get; set; } = string.Empty;
//     }
// }

// Models/AuthResponse.cs
using SettingsAPI.Models.DTOs;

namespace SettingsApp.Client.Models
{
    public class AuthResponse : UserDto
    {
        // public string Token { get; set; } = string.Empty;
        // public string Role { get; set; } = string.Empty;
        // public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

// Models/BasicSettings.cs
// namespace SettingsApp.Client.Models
// {
//     public class BasicSettings
//     {
//         public int Id { get; set; }
//         public string Name { get; set; } = string.Empty;
//         public string Value { get; set; } = string.Empty;
//         // Add other properties that match your API model
//     }
// }

// Models/AdvancedSettings.cs
// namespace SettingsApp.Client.Models
// {
//     public class AdvancedSettings
//     {
//         public int Id { get; set; }
//         public string Name { get; set; } = string.Empty;
//         public string Value { get; set; } = string.Empty;
//         public string Category { get; set; } = string.Empty;
//         // Add other properties that match your API model
//     }
// }