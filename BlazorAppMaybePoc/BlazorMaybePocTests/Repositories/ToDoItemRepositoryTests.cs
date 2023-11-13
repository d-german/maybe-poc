using System.Collections.Immutable;
using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Server.Repositories;
using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;
using Microsoft.EntityFrameworkCore;

namespace BlazorMaybePocTests.Repositories;

public class TestApplicationDbContext : DbContext, IApplicationDbContext // not using inheritance here since this "is a" DbContext and "can do" IApplicationDbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;

    public TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
        : base(options)
    {
    }

    public Task<Maybe<DbSet<ToDoItem>>> GetToDoItemsAsync()
    {
        return Task.FromResult<Maybe<DbSet<ToDoItem>>>(new Something<DbSet<ToDoItem>>(ToDoItems));
    }

    public async Task<Maybe<int>> PersistChangesAsync(CancellationToken cancellationToken = default)
    {
        return new Something<int>(await SaveChangesAsync(cancellationToken));
    }
}

[TestFixture]
public class ToDoItemRepositoryTests
{
    private ToDoItemRepository _repository = null!;
    private TestApplicationDbContext _dbContext = null!;

    [SetUp]
    public void Setup()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        _dbContext = new TestApplicationDbContext(options);
        _repository = new ToDoItemRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetToDoItemsAsync_ReturnsItems_WhenSuccessful()
    {
        var testItems = ImmutableList<ToDoItem>.Empty;
        _dbContext.ToDoItems.AddRange(testItems);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetToDoItemsAsync(new ToDoItemsRequest());

        Assert.That(result, Is.TypeOf<Something<IEnumerable<ToDoItem>>>());
        var something = result as Something<IEnumerable<ToDoItem>>;
        Assert.That(something?.Value, Is.EquivalentTo(testItems));
    }

    [Test]
    public async Task CreateAsync_AddsNewItem_WhenSuccessful()
    {
        var newToDoItem = new ToDoItem();

        var result = await _repository.CreateAsync(newToDoItem);

        Assert.That(result, Is.TypeOf<Something<ToDoItem>>());
        var something = result as Something<ToDoItem>;
        Assert.That(something?.Value, Is.EqualTo(newToDoItem));

        var itemInDb = await _dbContext.ToDoItems.FindAsync(newToDoItem.ToDoItemId);
        Assert.That(itemInDb, Is.Not.Null);
    }

    public static IEnumerable<TestCaseData> GetToDoItemsRequestTestCases
    {
        get
        {
            yield return new TestCaseData(
                new ToDoItemsRequest
                {
                    UserId = 1,
                    PrimarySortColumn = ToDoSortColumn.Priority,
                    SecondarySortColumn = ToDoSortColumn.DueDate,
                    SortAscending = true
                }).SetName("SortByPriorityThenDueDate_Ascending");

            yield return new TestCaseData(
                new ToDoItemsRequest
                {
                    UserId = 2,
                    PrimarySortColumn = ToDoSortColumn.Status,
                    SecondarySortColumn = ToDoSortColumn.Priority,
                    SortAscending = false
                }).SetName("SortByStatusThenPriority_Descending");
        }
    }

    [Test, TestCaseSource(nameof(GetToDoItemsRequestTestCases))]
    public async Task GetToDoItemsAsync_VariousRequests_ReturnsCorrectlySortedData(ToDoItemsRequest request)
    {
        var testItems = ImmutableList<ToDoItem>.Empty;
        _dbContext.ToDoItems.AddRange(testItems);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetToDoItemsAsync(request);

        var items = (result as Something<IEnumerable<ToDoItem>>)?.Value;
        Assert.That(items, Is.Not.Null);
    }
}