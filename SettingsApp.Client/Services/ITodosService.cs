// Services/ISettingsService.cs
using SettingsAPI.Models;
using SettingsApp.Client.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace SettingsApp.Client.Services
{
    public interface ITodosService
    {
        Task<Result<PagedResponse<TodoDto>>> GetTodos(int pageNumber = 1, int pageSize = 20, string? searchFilter = null);
        Task<TodoDto> CreateTodos();
        Task<Result<bool>> DeleteTodos(int id);
    }
}