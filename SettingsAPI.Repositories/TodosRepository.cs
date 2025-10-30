
using Microsoft.EntityFrameworkCore;
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public class TodosRepository : ITodosRepository
    {
        private readonly ApplicationDbContext _context;

        public TodosRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Todo>> GetAllTodosAsync(int pageNumber, int pageSize, string? searchFilter = null)
        {
            IQueryable<Todo> query = _context.Todos;

            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = ApplyAdvancedSearch(query, searchFilter);
            }

            return await query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(int id)
        {
            return await _context.Todos.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo?> DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return null;
            }

            _context.Todos.Remove(todo);
            _context.SaveChanges();
            return todo;
        }

        public async Task<int> GetTotalCountAsync(string? searchFilter = null)
        {
            IQueryable<Todo> query = _context.Todos;

            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = ApplyAdvancedSearch(query, searchFilter);
            }

            return await query.CountAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Applies advanced search filtering to the query.
        /// Supports GitLab-style search syntax:
        /// - "title=my task" - searches only in title field
        /// - "description=important" - searches only in description field  
        /// - "id=5" - searches for specific ID
        /// - "title=task description=good" - multiple field searches (AND logic)
        /// - "regular text" - searches in both title and description
        /// - "title=\"quoted text\"" - supports quoted values with spaces
        /// </summary>
        private IQueryable<Todo> ApplyAdvancedSearch(IQueryable<Todo> query, string searchFilter)
        {
            var searchTerms = ParseSearchFilter(searchFilter);
            
            foreach (var term in searchTerms)
            {
                switch (term.Field.ToLower())
                {
                    case "title":
                        query = query.Where(t => EF.Functions.Like(t.Title.ToLower(), $"%{term.Value.ToLower()}%"));
                        break;
                    case "description":
                        query = query.Where(t => t.Description != null && 
                                               EF.Functions.Like(t.Description.ToLower(), $"%{term.Value.ToLower()}%"));
                        break;
                    case "id":
                        if (int.TryParse(term.Value, out int id))
                        {
                            query = query.Where(t => t.Id == id);
                        }
                        break;
                    default:
                        // If no field specified or unknown field, search in both title and description
                        query = query.Where(t => EF.Functions.Like(t.Title.ToLower(), $"%{term.Value.ToLower()}%") || 
                                               (t.Description != null && 
                                                EF.Functions.Like(t.Description.ToLower(), $"%{term.Value.ToLower()}%")));
                        break;
                }
            }
            
            return query;
        }

        private List<SearchTerm> ParseSearchFilter(string searchFilter)
        {
            var terms = new List<SearchTerm>();
            
            // Split by spaces but preserve quoted strings
            var parts = SplitSearchString(searchFilter);
            
            foreach (var part in parts)
            {
                if (part.Contains('='))
                {
                    // Field-specific search: "title=task"
                    var fieldValue = part.Split('=', 2);
                    if (fieldValue.Length == 2)
                    {
                        terms.Add(new SearchTerm 
                        { 
                            Field = fieldValue[0].Trim(), 
                            Value = fieldValue[1].Trim().Trim('"') 
                        });
                    }
                }
                else
                {
                    // General search term
                    terms.Add(new SearchTerm 
                    { 
                        Field = "general", 
                        Value = part.Trim().Trim('"') 
                    });
                }
            }
            
            return terms;
        }

        private List<string> SplitSearchString(string input)
        {
            var parts = new List<string>();
            var current = "";
            var inQuotes = false;
            
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    current += c;
                }
                else if (c == ' ' && !inQuotes)
                {
                    if (!string.IsNullOrWhiteSpace(current))
                    {
                        parts.Add(current);
                        current = "";
                    }
                }
                else
                {
                    current += c;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(current))
            {
                parts.Add(current);
            }
            
            return parts;
        }

        private class SearchTerm
        {
            public string Field { get; set; } = "";
            public string Value { get; set; } = "";
        }
    }
}