using BlazorAppMaybePoc.Server.Controllers;
using BlazorAppMaybePoc.Server.Repositories;
using BlazorAppMaybePoc.Shared;
using BlazorAppMaybePoc.Shared.Functional;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMaybePocTests.ServerTests.Controllers;

[TestFixture]
public class ToDoItemControllerTests
{
    private Mock<IToDoItemRepository> _mockRepository = null!;
    private ToDoItemController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IToDoItemRepository>();
        _controller = new ToDoItemController(_mockRepository.Object);
    }

    [Test]
    public async Task Get_ReturnsOk_WhenToDoItemsFound()
    {
        var toDoItems = new List<ToDoItem>
        {
            new(),
            new()
        };

        _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(new Something<IEnumerable<ToDoItem>>(toDoItems));

        var result = await _controller.Get();

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(toDoItems));
    }

    [Test]
    public async Task Get_ReturnsNotFound_WhenNoToDoItems()
    {
        _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(new Nothing<IEnumerable<ToDoItem>>());

        var result = await _controller.Get();

        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Get_ReturnsInternalServerError_OnError()
    {
        _mockRepository.Setup(r => r.GetAsync()).ReturnsAsync(new Error<IEnumerable<ToDoItem>>(new Exception("Test error")));

        var result = await _controller.Get();

        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task Create_CallsRepositoryCreateAsync()
    {
        var newToDoItem = new ToDoItem();

        await _controller.Create(newToDoItem);

        _mockRepository.Verify(r => r.CreateAsync(newToDoItem), Times.Once);
    }

    [Test]
    public async Task GetByUserId_ReturnsOk_WhenToDoItemsFound()
    {
        var toDoItems = new List<ToDoItem>
        { /* Initialize ToDoItem instances */
        };
        var request = new ToDoItemsRequest
        { /* Initialize request properties */
        };
        _mockRepository.Setup(r => r.GetToDoItemsAsync(It.IsAny<ToDoItemsRequest>()))
            .ReturnsAsync(new Something<IEnumerable<ToDoItem>>(toDoItems));

        var result = await _controller.GetByUserId(request.UserId);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(toDoItems));
    }

    [Test]
    public async Task GetByUserId_ReturnsNotFound_WhenNoToDoItems()
    {
        _mockRepository.Setup(r => r.GetToDoItemsAsync(It.IsAny<ToDoItemsRequest>()))
            .ReturnsAsync(new Nothing<IEnumerable<ToDoItem>>());

        var result = await _controller.GetByUserId(1); // Example user ID

        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetByUserId_ReturnsInternalServerError_OnError()
    {
        _mockRepository.Setup(r => r.GetToDoItemsAsync(It.IsAny<ToDoItemsRequest>()))
            .ReturnsAsync(new Error<IEnumerable<ToDoItem>>(new Exception("Test error")));

        var result = await _controller.GetByUserId(1); // Example user ID

        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }
}