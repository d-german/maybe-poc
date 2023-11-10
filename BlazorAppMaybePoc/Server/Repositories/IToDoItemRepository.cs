using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;

namespace BlazorAppMaybePoc.Server.Repositories;

public interface IToDoItemRepository
{
    Task<Maybe<IEnumerable<ToDoItem>>> GetToDoItemsAsync(ToDoItemsRequest request);
    Task<Maybe<IEnumerable<ToDoItem>>> GetAsync();
    Task<Maybe<ToDoItem>> CreateAsync(ToDoItem newToDoItem);
}