using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppMaybePoc.Server.Repositories;

public class ToDoItemRepository : IToDoItemRepository
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ToDoItemRepository(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
    }

    public async Task<Maybe<IEnumerable<ToDoItem>>> GetToDoItemsAsync(ToDoItemsRequest request)
    {
        return (await _applicationDbContext.GetToDoItemsAsync()).Bind(items =>
            items.Where(item => item.UserId == request.UserId).ToList().AsEnumerable());
    }

    public async Task<Maybe<IEnumerable<ToDoItem>>> GetAsync()
    {
        return (await _applicationDbContext.GetToDoItemsAsync()).Bind(items => items.ToList().AsEnumerable());
    }

    public async Task<Maybe<ToDoItem>> CreateAsync(ToDoItem newToDoItem)
    {
        switch (await _applicationDbContext.GetToDoItemsAsync())
        {
            case Something<DbSet<ToDoItem>> something:
            {
                something.Value.Add(newToDoItem);
                return (await _applicationDbContext.PersistChangesAsync()).Bind(_ => new Something<ToDoItem>(newToDoItem));
            }
            case Error<DbSet<ToDoItem>> error:
                return new Error<ToDoItem>(error.ErrorMessage);
            default:
                return new Nothing<ToDoItem>();
        }
    }
}