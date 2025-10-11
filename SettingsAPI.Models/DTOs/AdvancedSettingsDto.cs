using System;

namespace SettingsAPI.Models.DTOs;

public class AdvancedSettingsDto
{
    public bool DebugModeEnabled { get; set; }
    public bool DetailedLoggingEnabled { get; set; }
    public bool BetaFeaturesEnabled { get; set; }
    public bool PerformanceMode { get; set; }
    public bool RemoteAccessEnabled { get; set; }
}
