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
        const string searchQuery = "Suppercalifragilisticexpialidocious";
        var searchResultCount = FullTextSearchService.SearchWithQueryParameter(searchQuery);
        Assert.That(searchResultCount, Is.EqualTo(5));
    }

    [Test]
    public void SearchWithShortStringQueryThrowsException()
    {
        const string searchQuery = "su";
        Assert.Throws<ArgumentException>(() => _ = FullTextSearchService.SearchWithQueryParameter(searchQuery));
    }

    [Test]
    public void CreatingSearchQueryWithShortStringThrowsException()
    {
        const string searchQuery = "su";
        Assert.Throws<ArgumentException>(() => _ = new SearchQuery { Query = searchQuery });
    }
}
