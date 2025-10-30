
// SettingsAPI.Repositories/ITodosRepository.cs
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public interface ITodosRepository
    {
        Task<List<Todo>> GetAllTodosAsync(int pageNumber, int pageSize, string? searchFilter = null);
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task<Todo?> DeleteTodoAsync(int id);
        Task<bool> SaveChangesAsync();
        Task<int> GetTotalCountAsync(string? searchFilter = null);
    }
}