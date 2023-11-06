namespace BlazorAppMaybePoc.Shared;

public record ToDoItemsRequest
{
    public int UserId { get; init; }
    public ToDoSortColumn PrimarySortColumn { get; init; }
    public ToDoSortColumn? SecondarySortColumn { get; init; }
    public bool SortAscending { get; init; }
}