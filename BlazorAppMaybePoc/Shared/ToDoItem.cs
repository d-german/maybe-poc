namespace BlazorAppMaybePoc.Shared;

public record ToDoItem
{
    public int ToDoItemId { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public Status Status { get; set; }
}