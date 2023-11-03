namespace BlazorAppMaybePoc.Shared;

public record User
{
    public int UserId { get; init; }
    public string? UserName { get; init; }
    public string? Password { get; init; }
    public string? Email { get; init; }
}