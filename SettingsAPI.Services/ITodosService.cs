
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;

namespace SettingsAPI.Services
{
    public interface ITodosService
    {
        Task<List<TodoDto>> GetTodosAsync(int pageNumber, int pageSize, string? searchFilter = null);
        Task<TodoDto> CreateTodoAsync(CreateTodoDto todo);
        Task<TodoDto?> GetTodoByIdAsync(int id);
        Task<TodoDto?> DeleteTodoAsync(int id);
        Task<int> GetTotalCountAsync(string? searchFilter = null);

    }
}