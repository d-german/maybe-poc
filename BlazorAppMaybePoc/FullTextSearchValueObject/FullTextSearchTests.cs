namespace FullTextSearchValueObject;

public class FullTextSearchTests
{
    [Test]
    public void SearchWithQueryReturnsExpectedResult()
    {
        var searchResultCount = FullTextSearchService.Search("Suppercalifragilisticexpialidocious");
        Assert.That(searchResultCount, Is.EqualTo(5));
    }

    [Test]
    public void SearchWithQueryObjectReturnsExpectedResult()
    {
        var searchQuery = new SearchQuery { Query = "Suppercalifragilisticexpialidocious" };
        var searchResultCount = FullTextSearchService.SearchWithQueryParameter(searchQuery);
        Assert.That(searchResultCount, Is.EqualTo(5));
    }

    [Test]
    public void SearchWithImplicitStringConversionReturnsExpectedResult()
    {
        // Implicit conversion from string to SearchQuery
        // public static implicit operator SearchQuery(string query) => new() { Query = query };
        const string searchQuery = "Suppercalifragilisticexpialidocious";
        var searchResultCount = FullTextSearchService.SearchWithQueryParameter(searchQuery);
        Assert.That(searchResultCount, Is.EqualTo(5));
    }

    [Test]
    public void Test4()
    {
        const string searchQuery = "su";
        Assert.Throws<ArgumentException>(() => _ = FullTextSearchService.SearchWithQueryParameter(searchQuery));
    }

    [Test]
    public void Test5()
    {
        const string searchQuery = "su";
        Assert.Throws<ArgumentException>(() => _ = new SearchQuery { Query = searchQuery });
    }
}
