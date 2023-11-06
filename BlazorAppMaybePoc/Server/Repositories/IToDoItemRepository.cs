using BlazorAppMaybePoc.Shared;

namespace BlazorAppMaybePoc.Server.Repositories;

public interface IToDoItemRepository
{
    Task<IEnumerable<ToDoItem>> GetToDoItemsAsync(ToDoItemsRequest request);
    Task<IEnumerable<ToDoItem>> GetAsync();
    Task CreateAsync(ToDoItem newToDoItem);
}