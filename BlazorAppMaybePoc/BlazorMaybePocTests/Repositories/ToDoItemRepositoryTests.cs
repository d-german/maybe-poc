using System.Collections.Immutable;
using BlazorAppMaybePoc.Server.Data;
using BlazorAppMaybePoc.Server.Repositories;
using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;
using Microsoft.EntityFrameworkCore;

namespace BlazorMaybePocTests.Repositories;

public class TestApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;

    public TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
        : base(options)
    {
    }

    public async Task<Maybe<DbSet<ToDoItem>>> GetToDoItemsAsync()
    {
        return new Something<DbSet<ToDoItem>>(ToDoItems);
    }

    public async Task<Maybe<int>> PersistChangesAsync(CancellationToken cancellationToken = default)
    {
        return new Something<int>(await SaveChangesAsync(cancellationToken));
    }
}

[TestFixture]
public class ToDoItemRepositoryTests
{
    private ToDoItemRepository _repository;
    private TestApplicationDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        // Ensure a unique database name for each test run
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
        _dbContext.Database.EnsureDeleted(); // This ensures the in-memory database is deleted
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
}