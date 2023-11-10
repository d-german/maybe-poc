using BlazorAppMaybePoc.Server.Repositories;
using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;
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
    public async Task<IActionResult> Get()
    {
        var result = await _toDoItemRepository.GetAsync();
        return result switch
        {
            Nothing<IEnumerable<ToDoItem>> _ => NotFound("No ToDo items found!"),
            Something<IEnumerable<ToDoItem>> s => Ok(s.Value),
            Error<IEnumerable<ToDoItem>> e => StatusCode(500, e.ErrorMessage.Message),
            _ => StatusCode(500, "An unknown error occurred")
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToDoItem newToDoItem)
    {
        await _toDoItemRepository.CreateAsync(newToDoItem);
        return Ok();
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId, string primarySortColumn = "", string secondarySortColumn = "", bool sortAscending = true)
    {
        var request = new ToDoItemsRequest
        {
            UserId = userId,
            PrimarySortColumn = primarySortColumn.StringToEnum(ToDoSortColumn.DueDate),
            SecondarySortColumn = secondarySortColumn.StringToEnum<ToDoSortColumn>(),
            SortAscending = sortAscending
        };

        return await _toDoItemRepository.GetToDoItemsAsync(request) switch
        {
            Nothing<IEnumerable<ToDoItem>> _ => NotFound("No ToDo items found for the user."),
            Something<IEnumerable<ToDoItem>> s => Ok(s.Value),
            Error<IEnumerable<ToDoItem>> e => StatusCode(500, e.ErrorMessage.Message),
            _ => StatusCode(500, "An unknown error occurred")
        };
    }
}