using System;

namespace SettingsAPI.Models;

public class BasicSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public bool EnableNotifications { get; set; } = true;
    public bool DarkModeEnabled { get; set; } = false;
    public bool AutoSaveEnabled { get; set; } = true;
    public bool SoundEnabled { get; set; } = true;
}
