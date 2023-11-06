using BlazorAppMaybePoc.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Data;

public class ToDoItemDataAccess : IToDoItemDataAccess
{
    private readonly DbContext _dbContext;

    public ToDoItemDataAccess(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<ToDoItem> ToDoItems => _dbContext.Set<ToDoItem>();

    public Task<int> PersistChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}