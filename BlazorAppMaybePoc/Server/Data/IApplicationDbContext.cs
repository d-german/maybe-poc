using BlazorAppMaybePoc.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Data;

public interface IApplicationDbContext
{
    DbSet<ToDoItem> ToDoItems { get; }
    Task<int> PersistChangesAsync(CancellationToken cancellationToken = default);
}