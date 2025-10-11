// Services/ISettingsService.cs
using SettingsAPI.Models;
using SettingsApp.Client.Models;

namespace SettingsApp.Client.Services
{
    public interface ISettingsService
    {
        Task<BasicSettings> GetBasicSettings();
        Task<AdvancedSettings> GetAdvancedSettings();
        Task<BasicSettings> UpdateBasicSetting(BasicSettings setting);
        Task<AdvancedSettings> UpdateAdvancedSetting(AdvancedSettings setting);
    }
}