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
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [HttpGet]
    public async Task<IEnumerable<ToDoItem>> Get()
    {
        return await _dbContext.ToDoItems!.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItem newToDoItem)
    {
        _dbContext.ToDoItems?.Add(newToDoItem);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("user/{userId}")]
    public async Task<IEnumerable<ToDoItem>> GetByUserId(int userId, string primarySortColumn = "", string secondarySortColumn = "", bool sortAscending = true)
    {
        var query = _dbContext.ToDoItems!.Where(item => item.UserId == userId);

        // Primary sort
        switch (primarySortColumn)
        {
            case nameof(ToDoItem.Priority):
                query = sortAscending ? query.OrderBy(item => item.Priority) : query.OrderByDescending(item => item.Priority);
                break;
            case nameof(ToDoItem.Status):
                query = sortAscending ? query.OrderBy(item => item.Status) : query.OrderByDescending(item => item.Status);
                break;
            case nameof(ToDoItem.DueDate):
                query = sortAscending ? query.OrderBy(item => item.DueDate) : query.OrderByDescending(item => item.DueDate);
                break;
        }

        // Secondary sort
        if (!string.IsNullOrEmpty(secondarySortColumn) && secondarySortColumn != primarySortColumn)
        {
            switch (secondarySortColumn)
            {
                case nameof(ToDoItem.Priority):
                    query = sortAscending ? ((IOrderedQueryable<ToDoItem>)query).ThenBy(item => item.Priority) : ((IOrderedQueryable<ToDoItem>)query).ThenByDescending(item => item.Priority);
                    break;
                case nameof(ToDoItem.Status):
                    query = sortAscending ? ((IOrderedQueryable<ToDoItem>)query).ThenBy(item => item.Status) : ((IOrderedQueryable<ToDoItem>)query).ThenByDescending(item => item.Status);
                    break;
                case nameof(ToDoItem.DueDate):
                    query = sortAscending ? ((IOrderedQueryable<ToDoItem>)query).ThenBy(item => item.DueDate) : ((IOrderedQueryable<ToDoItem>)query).ThenByDescending(item => item.DueDate);
                    break;
            }
        }

        return await query.ToListAsync();
    }
}