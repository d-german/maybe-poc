using System.Net.Http.Json;
using BlazorAppMaybePoc.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorAppMaybePoc.Client.Pages;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class UserTasks : ComponentBase
{
    private IEnumerable<ToDoItem> _toDoItems = null!;
    private string _primarySortColumn = nameof(ToDoItem.Priority);
    private string _secondarySortColumn = nameof(ToDoItem.DueDate);
    private bool _sortAscending = true;
    private readonly ToDoItemViewModel _viewModel = new();

    private string _currentSortColumn = null!;
    private bool _currentSortAscending;

    [Inject]
    HttpClient Http { get; init; }

    private Task LoadTasks()
    {
        return SortTasks(_primarySortColumn);
    }

    private Task SortTasks(string column)
    {
        if (_currentSortColumn == column)
        {
            _currentSortAscending = !_currentSortAscending;
        }
        else
        {
            _currentSortColumn = column;
            _currentSortAscending = true;
        }

        if (_primarySortColumn == column)
        {
            _sortAscending = !_sortAscending;
        }
        else
        {
            _secondarySortColumn = _primarySortColumn;
            _primarySortColumn = column;
            _sortAscending = true;
        }

        return FetchSortedData();
    }

    private async Task FetchSortedData()
    {
        var url = $"ToDoItem/user/{_viewModel.UserId}?primarySortColumn={_primarySortColumn}&secondarySortColumn={_secondarySortColumn}&sortAscending={_sortAscending}";
        var response = await Http.GetAsync(url);
        _toDoItems = (await response.Content.ReadFromJsonAsync<IEnumerable<ToDoItem>>())!;
    }

    private async Task HandleCreate()
    {
        var newToDoItem = _viewModel.ToToDoItem();
        var response = await Http.PostAsJsonAsync("ToDoItem", newToDoItem);
        if (response.IsSuccessStatusCode)
        {
            await LoadTasks();
        }
    }

    private string GetSortIconClass(string columnName)
    {
        if (_currentSortColumn != columnName)
        {
            return "oi oi-elevator"; // This class is for the default icon when no sorting is applied
        }

        return _currentSortAscending ? "oi oi-arrow-thick-bottom" : "oi oi-arrow-thick-top";
    }

    private class ToDoItemViewModel
    {
        public string UserId { get; set; } = "1";
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.NotStarted;
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime DueDate { get; set; } = DateTime.Today;

        public bool IsValid => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Description);

        public ToDoItem ToToDoItem() => new ToDoItem
            {
                Title = Title,
                Description = Description,
                DueDate = DueDate,
                Priority = Priority,
                Status = Status
            } with
            {
                UserId = int.Parse(UserId)
            };
    }
}