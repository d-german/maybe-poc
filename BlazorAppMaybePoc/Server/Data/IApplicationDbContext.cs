using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Data;

public interface IApplicationDbContext
{
    Task<Maybe<DbSet<ToDoItem>>> GetToDoItemsAsync();
    Task<Maybe<int>> PersistChangesAsync(CancellationToken cancellationToken = default);
}