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
    private readonly ToDoItemFormModel _formModel = new();

    [Inject]
    HttpClient Http { get; init; }

    private Task LoadTasks()
    {
        return SortTasks(_primarySortColumn);
    }

    private Task SortTasks(string column)
    {
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
        var url = $"ToDoItem/user/{_formModel.UserId}?primarySortColumn={_primarySortColumn}&secondarySortColumn={_secondarySortColumn}&sortAscending={_sortAscending}";
        var response = await Http.GetAsync(url);
        _toDoItems = await response.Content.ReadFromJsonAsync<IEnumerable<ToDoItem>>();
    }

    private async Task HandleCreate()
    {
        var newToDoItem = _formModel.ToToDoItem();
        var response = await Http.PostAsJsonAsync("ToDoItem", newToDoItem);
        if (response.IsSuccessStatusCode)
        {
            await LoadTasks();
        }
    }

    private class ToDoItemFormModel
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }

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