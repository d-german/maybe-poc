using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Shared;
using Microsoft.EntityFrameworkCore;
using static BlazorAppMaybePoc.Shared.ToDoSortColumn;

namespace BlazorAppMaybePoc.Server.Repositories;

public class ToDoItemRepository : IToDoItemRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ToDoItemRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<ToDoItem>> GetToDoItemsAsync(ToDoItemsRequest request)
    {
        var query = _dbContext.ToDoItems!.Where(item => item.UserId == request.UserId);

        // Primary sort
        query = request.PrimarySortColumn switch
        {
            ToDoSortColumn.Priority => request.SortAscending ? query.OrderBy(item => item.Priority) : query.OrderByDescending(item => item.Priority),
            ToDoSortColumn.Status => request.SortAscending ? query.OrderBy(item => item.Status) : query.OrderByDescending(item => item.Status),
            DueDate => request.SortAscending ? query.OrderBy(item => item.DueDate) : query.OrderByDescending(item => item.DueDate),
            _ => query.OrderBy(item => item.DueDate) // Default sort, if needed
        };

        // Secondary sort
        if (request.SecondarySortColumn.HasValue && request.SecondarySortColumn.Value != request.PrimarySortColumn)
        {
            query = request.SecondarySortColumn switch
            {
                ToDoSortColumn.Priority => request.SortAscending ? ((IOrderedQueryable<ToDoItem>)query).ThenBy(item => item.Priority) : ((IOrderedQueryable<ToDoItem>)query).ThenByDescending(item => item.Priority),
                ToDoSortColumn.Status => request.SortAscending ? ((IOrderedQueryable<ToDoItem>)query).ThenBy(item => item.Status) : ((IOrderedQueryable<ToDoItem>)query).ThenByDescending(item => item.Status),
                DueDate => request.SortAscending ? ((IOrderedQueryable<ToDoItem>)query).ThenBy(item => item.DueDate) : ((IOrderedQueryable<ToDoItem>)query).ThenByDescending(item => item.DueDate),
                _ => query // No secondary sort if it's the same as the primary or not present
            };
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<ToDoItem>> GetAsync()
    {
        return await _dbContext.ToDoItems!.ToListAsync();
    }

    public async Task CreateAsync(ToDoItem newToDoItem)
    {
        _dbContext.ToDoItems?.Add(newToDoItem);
        await _dbContext.SaveChangesAsync();
    }
}