using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Repositories;

namespace SettingsAPI.Services
{
    public class TodosService : ITodosService
    {
        private readonly ITodosRepository _repository;

        public TodosService(ITodosRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TodoDto>> GetTodosAsync(int pageNumber, int pageSize, string? searchFilter = null)
        {
            var todos = await _repository.GetAllTodosAsync(pageNumber, pageSize, searchFilter);
            var listDtos = todos.Select(dto => new TodoDto
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description
            }).ToList();

            return listDtos;
        }

        public async Task<TodoDto?> GetTodoByIdAsync(int id)
        {
            var todo = await _repository.GetTodoByIdAsync(id);

            if (todo == null)
                return null;

            return new TodoDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description
            };
        }

        public async Task<TodoDto> CreateTodoAsync(CreateTodoDto todo)
        {
            var new_todo = await _repository.CreateTodoAsync(new Todo
            {
                Title = todo.Title,
                Description = todo.Description
            });

            return new TodoDto
            {
                Id = new_todo.Id,
                Title = new_todo.Title,
                Description = new_todo.Description
            };
        }

        public async Task<TodoDto?> DeleteTodoAsync(int id)
        {
            var del_todo = await _repository.DeleteTodoAsync(id);

            if (del_todo == null)
                return null;

            return new TodoDto
            {
                Id = del_todo.Id,
                Title = del_todo.Title,
                Description = del_todo.Description
            };
        }
        public async Task<int> GetTotalCountAsync(string? searchFilter = null)
        {
            return await _repository.GetTotalCountAsync(searchFilter);
        }
    }
}