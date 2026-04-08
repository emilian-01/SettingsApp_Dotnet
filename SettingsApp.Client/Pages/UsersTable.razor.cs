using Microsoft.AspNetCore.Components;
using SettingsApp.Client.Models;
using SettingsApp.Client.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SettingsApp.Client.Pages
{
    public partial class UsersTable
    {
        [Inject]
        private IUsersService UsersService { get; set; }

        private List<User> users;
        private Dictionary<int, string> originalEmails = new Dictionary<int, string>();

        protected override async Task OnInitializedAsync()
        {
            users = await UsersService.GetUsers();
        }

        private void EditUser(User user)
        {
            originalEmails[user.Id] = user.Email;
            user.IsEditing = true;
        }

        private async Task SaveUser(User user)
        {
            user.IsEditing = false;
            await UsersService.UpdateUser(user);
            originalEmails.Remove(user.Id);
        }

        private void CancelEdit(User user)
        {
            user.Email = originalEmails[user.Id];
            user.IsEditing = false;
            originalEmails.Remove(user.Id);
        }
    }
}
