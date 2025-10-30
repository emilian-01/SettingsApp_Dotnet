using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SettingsAPI.Models;
using SettingsAPI.Models.DTOs;
using SettingsAPI.Services;
using SettingsAPI.Utilities;

namespace SettingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires authentication
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;

        public TodosController(ITodosService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoDto>>> GetAll([FromQuery] PaginationFilter filter, [FromQuery] string? searchFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var totalRecords = await _todosService.GetTotalCountAsync(searchFilter);
            var todos = await _todosService.GetTodosAsync(validFilter.PageNumber, validFilter.PageSize, searchFilter);
            var pagedResponse = new PagedResponse<TodoDto>(todos, validFilter.PageNumber, validFilter.PageSize, totalRecords);

            return Ok(pagedResponse);
        }


        [HttpPost]
        public async Task<ActionResult<TodoDto>> Create([FromBody] CreateTodoDto todoDto)
        {
            var todo = await _todosService.CreateTodoAsync(todoDto);
            return Ok(todo);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<TodoDto>> Delete([FromRoute] int id)
        {
            var todo = await _todosService.DeleteTodoAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }
    }
}