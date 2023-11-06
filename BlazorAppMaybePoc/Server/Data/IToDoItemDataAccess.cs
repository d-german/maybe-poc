using BlazorAppMaybePoc.Shared;

namespace BlazorAppMaybePoc.Server.Data;

public interface IToDoItemDataAccess
{
    IEnumerable<ToDoItem> ToDoItems { get; }
    Task<int> PersistChangesAsync(CancellationToken cancellationToken = default);
}