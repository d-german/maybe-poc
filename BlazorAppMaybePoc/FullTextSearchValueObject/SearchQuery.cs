namespace FullTextSearchValueObject;

public record SearchQuery
{
    private readonly string _query = null!;

    public required string Query
    {
        get => _query;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Query cannot be null or whitespace.", nameof(Query));
            }

            if (value.Length < 3)
            {
                throw new ArgumentException("Query must be at least 3 characters long.", nameof(Query));
            }

            _query = value;
        }
    }

    // Implicit conversion from string to SearchQuery
    public static implicit operator SearchQuery(string query) => new() { Query = query };
}
