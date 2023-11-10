using BlazorAppMaybePoc.Shared;

namespace BlazorAppMaybePoc.Client.Pages;

public class ToDoItemViewModel
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