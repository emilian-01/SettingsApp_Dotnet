using System;

namespace SettingsAPI.Models.DTOs;

public class BasicSettingsDto
{
    public bool EnableNotifications { get; set; }
    public bool DarkModeEnabled { get; set; }
    public bool AutoSaveEnabled { get; set; }
    public bool SoundEnabled { get; set; }
}
