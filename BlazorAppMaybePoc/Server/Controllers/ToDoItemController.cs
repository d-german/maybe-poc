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

    // ... other actions ...
}