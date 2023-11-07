using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Data;

public class ApplicationDbContextAdapter : IApplicationDbContext
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationDbContextAdapter(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<Maybe<DbSet<ToDoItem>>> GetToDoItemsAsync()
    {
        try
        {
            return Task.FromResult<Maybe<DbSet<ToDoItem>>>(new Something<DbSet<ToDoItem>>(_dbContext.Set<ToDoItem>()));
        }
        catch (Exception ex)
        {
            return Task.FromResult<Maybe<DbSet<ToDoItem>>>(new Error<DbSet<ToDoItem>>(ex));
        }
    }

    public async Task<Maybe<int>> PersistChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return new Something<int>(await _dbContext.SaveChangesAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            return new Error<int>(ex);
        }
    }
}