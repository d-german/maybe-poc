using BlazorAppMaybePoc.Client.Pages;

namespace BlazorMaybePocTests;

public class UserTasksTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task LoadTasksAsync_CallsSortTasksAsyncWithCorrectArgument()
    {
        var mockUserTasks = new Mock<UserTasks>
        {
            CallBase = true
        };
        mockUserTasks.Setup(x => x.SortTasksAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        await mockUserTasks.Object.LoadTasksAsync();

        mockUserTasks.Verify(x => x.SortTasksAsync("Priority"), Times.Once);
    }

    [Test]
    public async Task SortTasksAsync_ConstructsCorrectUrl()
    {
        var mockHttp = new MockHttpMessageHandler();

        const string baseUrl = "https://example.com/";
        var httpClient = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri(baseUrl)
        };

        const string columnToSortBy = "DueDate";
        const string userId = "1";
        const string secondarySortColumn = "Priority";
        const bool sortAscending = true;

        var expectedUrl = $"https://example.com/ToDoItem/user/{userId}?primarySortColumn={columnToSortBy}&secondarySortColumn={secondarySortColumn}&sortAscending={sortAscending}";
        mockHttp.When(expectedUrl)
            .Respond("application/json", "[]");

        var userTasks = new UserTasks
        {
            Http = httpClient
        };

        await userTasks.SortTasksAsync(columnToSortBy);

        mockHttp.VerifyNoOutstandingExpectation();
    }
}