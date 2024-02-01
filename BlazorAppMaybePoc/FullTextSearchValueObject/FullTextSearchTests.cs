namespace FullTextSearchValueObject;

public class FullTextSearchTests
{
    [Test]
    public void Test1()
    {
        var searchResultCount = FullTextSearchService.Search("Suppercalifragilisticexpialidocious");
        Assert.That(searchResultCount, Is.EqualTo(5));
    }
    
    [Test]
    public void Test2()
    {
        var searchQuery = new SearchQuery { Query = "Suppercalifragilisticexpialidocious" };
        var searchResultCount = FullTextSearchService.Search(searchQuery);
        Assert.That(searchResultCount, Is.EqualTo(5));
    }
}
