using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;
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
        return (await _applicationDbContext.GetToDoItemsAsync())
            .Bind(items => items.Where(item => item.UserId == request.UserId))
            .Bind(items =>
            {
                // Primary sort
                var sortedItems = request.PrimarySortColumn switch
                {
                    ToDoSortColumn.Priority => request.SortAscending ? items.OrderBy(item => item.Priority) : items.OrderByDescending(item => item.Priority),
                    ToDoSortColumn.Status => request.SortAscending ? items.OrderBy(item => item.Status) : items.OrderByDescending(item => item.Status),
                    ToDoSortColumn.DueDate => request.SortAscending ? items.OrderBy(item => item.DueDate) : items.OrderByDescending(item => item.DueDate),
                    _ => items.OrderBy(item => item.DueDate) // Default sort, if needed
                };

                // Secondary sort
                if (request.SecondarySortColumn.HasValue && request.SecondarySortColumn.Value != request.PrimarySortColumn)
                {
                    sortedItems = request.SecondarySortColumn switch
                    {
                        ToDoSortColumn.Priority => request.SortAscending ? sortedItems.ThenBy(item => item.Priority) : sortedItems.ThenByDescending(item => item.Priority),
                        ToDoSortColumn.Status => request.SortAscending ? sortedItems.ThenBy(item => item.Status) : sortedItems.ThenByDescending(item => item.Status),
                        ToDoSortColumn.DueDate => request.SortAscending ? sortedItems.ThenBy(item => item.DueDate) : sortedItems.ThenByDescending(item => item.DueDate),
                        _ => sortedItems // No secondary sort if it's the same as the primary or not present
                    };
                }

                // The ToMaybe() extension method wraps the sortedItems back into a Maybe
                return sortedItems.AsEnumerable().ToMaybe();
            });
    }

    public async Task<Maybe<IEnumerable<ToDoItem>>> GetAsync()
    {
        return (await _applicationDbContext.GetToDoItemsAsync()).Bind(items => items.AsEnumerable());
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