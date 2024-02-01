namespace FullTextSearchValueObject;

public class FullTextSearchService
{
    public static int Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be null or empty", nameof(query));
        }

        if (query.Length < 3)
        {
            throw new ArgumentException("Query must be at least 3 characters long", nameof(query));
        }

        return PerformSearch(query);
    }

    public static int SearchWithQueryParameter(SearchQuery query)
    {
        return PerformSearch(query.Query);
    }

    private static int PerformSearch(string query) => 5;
}
