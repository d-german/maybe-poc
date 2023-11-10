using System.Net.Http.Json;
using BlazorAppMaybePoc.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorAppMaybePoc.Client.Pages;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class UserTasks : ComponentBase
{
    public IEnumerable<ToDoItem> ToDoItems { get; private set; } = null!;
    public string PrimarySortColumn { get; private set; } = nameof(ToDoItem.Priority);
    public string SecondarySortColumn { get; private set; } = nameof(ToDoItem.DueDate);
    public bool SortAscending { get; private set; } = true;
    public ToDoItemViewModel ViewModel { get; } = new();

    private string _currentSortColumn = null!;
    private bool _currentSortAscending;

    [Inject]
    public HttpClient Http { get; init; }

    public Task LoadTasksAsync()
    {
        return SortTasksAsync(PrimarySortColumn);
    }

    public async Task SortTasksAsync(string column)
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

        if (PrimarySortColumn == column)
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SecondarySortColumn = PrimarySortColumn;
            PrimarySortColumn = column;
            SortAscending = true;
        }

        await FetchDataAsync();
    }

    public async Task FetchDataAsync()
    {
        var url = $"ToDoItem/user/{ViewModel.UserId}?primarySortColumn={PrimarySortColumn}&secondarySortColumn={SecondarySortColumn}&sortAscending={SortAscending}";
        var response = await Http.GetAsync(url);
        ToDoItems = (await response.Content.ReadFromJsonAsync<IEnumerable<ToDoItem>>())!;
    }

    public async Task CreateAsync()
    {
        var newToDoItem = ViewModel.ToToDoItem();
        var response = await Http.PostAsJsonAsync("ToDoItem", newToDoItem);
        if (response.IsSuccessStatusCode)
        {
            await LoadTasksAsync();
        }
    }

    public string GetSortIconClass(string columnName)
    {
        if (_currentSortColumn != columnName)
        {
            return "oi oi-elevator"; // This class is for the default icon when no sorting is applied
        }

        return _currentSortAscending ? "oi oi-arrow-thick-bottom" : "oi oi-arrow-thick-top";
    }
}