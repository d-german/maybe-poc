using System.Net;
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

    [TestCase("DueDate", true, "https://example.com/ToDoItem/user/1?primarySortColumn=DueDate&secondarySortColumn=Priority&sortAscending=True")]
    [TestCase("Priority", false, "https://example.com/ToDoItem/user/1?primarySortColumn=Priority&secondarySortColumn=DueDate&sortAscending=False")]
    public async Task SortTasksAsync_ConstructsCorrectUrl(string columnToSortBy, bool initialSortAscending, string expectedUrl)
    {
        var mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri("https://example.com/")
        };

        var userTasks = new UserTasks
        {
            Http = httpClient
        };
        userTasks.ViewModel.UserId = "1";
        userTasks.PrimarySortColumn = "Priority"; // Set to Priority initially
        userTasks.SecondarySortColumn = "DueDate";
        userTasks.SortAscending = !initialSortAscending; // Set opposite to force toggle

        mockHttp.When(expectedUrl).Respond("application/json", "[]");

        await userTasks.SortTasksAsync(columnToSortBy);

        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Test]
    public async Task SortTasksAsync_TogglesSortOrder_WhenSameColumnSortedTwice()
    {
        var mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri("https://example.com/")
        };

        var userTasks = new UserTasks
        {
            Http = httpClient
        };
        userTasks.ViewModel.UserId = "1";
        userTasks.PrimarySortColumn = "Priority";
        userTasks.SecondarySortColumn = "DueDate";
        userTasks.SortAscending = true;

        // Mock setup for the expected URLs
        mockHttp.When("https://example.com/ToDoItem/user/1?primarySortColumn=DueDate&secondarySortColumn=Priority&sortAscending=True").Respond("application/json", "[]");
        mockHttp.When("https://example.com/ToDoItem/user/1?primarySortColumn=DueDate&secondarySortColumn=Priority&sortAscending=False").Respond("application/json", "[]");

        // First call with a different column to set _currentSortColumn implicitly
        await userTasks.SortTasksAsync("DueDate");

        // Second call with the same column to toggle sort order
        await userTasks.SortTasksAsync("DueDate");

        // Verify that SortAscending is toggled
        Assert.That(userTasks.SortAscending, Is.False);

        // Third call with the same column to toggle sort order back
        await userTasks.SortTasksAsync("DueDate");

        // Verify that SortAscending is toggled back
        Assert.That(userTasks.SortAscending, Is.True);
    }

    [Test]
    public async Task CreateAsync_SendsPostRequest_AndLoadsTasksOnSuccess()
    {
        var mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri("https://example.com/")
        };
        var userTasks = new UserTasks
        {
            Http = httpClient,
            ViewModel = new ToDoItemViewModel()
        };

        // Setup the ViewModel
        // userTasks.ViewModel.Property1 = value1; // Set necessary properties for the ViewModel

        var postRequests = new List<HttpRequestMessage>();
        var getRequests = new List<HttpRequestMessage>();

        // Mock setup for POST request
        mockHttp.When(HttpMethod.Post, "https://example.com/ToDoItem")
            .Respond(req =>
            {
                postRequests.Add(req);
                return new HttpResponseMessage(HttpStatusCode.Created);
            });

        // Mock setup for subsequent GET request in LoadTasksAsync
        mockHttp.When(HttpMethod.Get, "*").Respond(req =>
        {
            getRequests.Add(req);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]")
            };
        });

        await userTasks.CreateAsync();

        // Verify POST request was made with correct data
        Assert.That(postRequests.Any(req => req.Method == HttpMethod.Post && req.RequestUri.ToString().Contains("ToDoItem")), Is.True);

        // Verify LoadTasksAsync was called
        Assert.That(getRequests.Any(req => req.Method == HttpMethod.Get), Is.True);
    }

    [Test]
    [TestCase("Priority", "Priority", true, "oi oi-arrow-thick-bottom")]
    [TestCase("DueDate", "Priority", true, "oi oi-elevator")]
    public async Task GetSortIconClass_ReturnsCorrectIconClass(string initialSortColumn, string columnName, bool initialSortAscending, string expectedClass)
    {
        var userTasks = new UserTasks();
        userTasks.ViewModel = new ToDoItemViewModel(); // Assuming ViewModel is required
        userTasks.PrimarySortColumn = initialSortColumn;
        userTasks.SortAscending = initialSortAscending;

        // Use a mock HTTP client as SortTasksAsync may trigger an HTTP request
        var mockHttp = new MockHttpMessageHandler();
        userTasks.Http = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri("https://example.com/")
        };
        mockHttp.When("*").Respond("application/json", "[]"); // General mock response

        // Call SortTasksAsync to set _currentSortColumn and _currentSortAscending
        await userTasks.SortTasksAsync(initialSortColumn);

        var result = userTasks.GetSortIconClass(columnName);

        Assert.That(result, Is.EqualTo(expectedClass));
    }
}