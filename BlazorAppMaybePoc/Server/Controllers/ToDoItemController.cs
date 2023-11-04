using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoItemController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ToDoItemController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IEnumerable<ToDoItem>> Get()
    {
        return await _dbContext.ToDoItems.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItem newToDoItem)
    {
        _dbContext.ToDoItems.Add(newToDoItem);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("user/{userId}")]
    public async Task<IEnumerable<ToDoItem>> GetByUserId(int userId, string sortColumn = "", bool sortAscending = true)
    {
        var query = _dbContext.ToDoItems.Where(item => item.UserId == userId);

        if (!string.IsNullOrEmpty(sortColumn))
        {
            if (sortColumn == "Priority")
            {
                query = sortAscending ? query.OrderBy(item => item.Priority) : query.OrderByDescending(item => item.Priority);
            }
            else if (sortColumn == "Status")
            {
                query = sortAscending ? query.OrderBy(item => item.Status) : query.OrderByDescending(item => item.Status);
            }
        }

        return await query.ToListAsync();
    }
}