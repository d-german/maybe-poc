using BlazorAppMaybePoc.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Data;

public class ApplicationDbContextAdapter : IApplicationDbContext
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationDbContextAdapter(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public DbSet<ToDoItem> ToDoItems => _dbContext.Set<ToDoItem>();

    public Task<int> PersistChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}