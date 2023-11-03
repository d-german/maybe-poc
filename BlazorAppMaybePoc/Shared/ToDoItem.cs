namespace BlazorAppMaybePoc.Shared;

public record ToDoItem
{
    public int ToDoItemId { get; init; }
    public int UserId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime DueDate { get; init; }
    public Priority Priority { get; init; }
    public Status Status { get; init; }
}