using BlazorAppMaybePoc.Server.Repositories;
using BlazorAppMaybePoc.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppMaybePoc.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoItemController : ControllerBase
{
    private readonly IToDoItemRepository _toDoItemRepository;

    public ToDoItemController(IToDoItemRepository toDoItemRepository)
    {
        _toDoItemRepository = toDoItemRepository ?? throw new ArgumentNullException(nameof(toDoItemRepository));
    }

    [HttpGet]
    public Task<IEnumerable<ToDoItem>> Get()
    {
        return _toDoItemRepository.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItem newToDoItem)
    {
        await _toDoItemRepository.CreateAsync(newToDoItem);
        return Ok();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetByUserId(int userId, string primarySortColumn = "", string secondarySortColumn = "", bool sortAscending = true)
    {
        var request = new ToDoItemsRequest
        {
            UserId = userId,
            PrimarySortColumn = primarySortColumn.StringToEnum(ToDoSortColumn.DueDate),
            SecondarySortColumn = secondarySortColumn.StringToEnum<ToDoSortColumn>(),
            SortAscending = sortAscending
        };

        try
        {
            return Ok(await _toDoItemRepository.GetToDoItemsAsync(request));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}