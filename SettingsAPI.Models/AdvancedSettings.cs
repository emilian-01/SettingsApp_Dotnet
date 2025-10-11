using System;

namespace SettingsAPI.Models;

public class AdvancedSettings
{
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public bool DebugModeEnabled { get; set; } = false;
        public bool DetailedLoggingEnabled { get; set; } = false;
        public bool BetaFeaturesEnabled { get; set; } = false;
        public bool PerformanceMode { get; set; } = false;
        public bool RemoteAccessEnabled { get; set; } = false;
}
