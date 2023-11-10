using BlazorAppMaybePoc.Client;
using BlazorAppMaybePoc.Shared.Common;
using static BlazorAppMaybePoc.Client.TemperatureConversionServiceMaybe;

namespace BlazorMaybePocTests.ClientTests;

[TestFixture]
public class TemperatureConversionServiceMaybeTests
{
    [TestCase(32, "0°C")]
    [TestCase(212, "100°C")]
    // More test cases
    public void FahrenheitToCelsius_ReturnsExpectedResult(decimal fahrenheit, string expected)
    {
        var result = FahrenheitToCelsius(fahrenheit);

        Assert.That(result, Is.TypeOf<Something<string>>(), "Result should be a Something<string> type");
        var something = result as Something<string>;
        Assert.That(something?.Value, Is.EqualTo(expected));
    }

    [Test]
    public void FahrenheitToCelsius_WhenMagicValue_ReturnsError()
    {
        var result = FahrenheitToCelsius(42);

        Assert.That(result, Is.TypeOf<Error<string>>(), "Result should be an Error<string> type");
        var error = result as Error<string>;
        Assert.That(error?.ErrorMessage, Is.TypeOf<UniverseAnswerTemperatureException>());
    }

    [TestCase(0, "32°F")]
    [TestCase(100, "212°F")]
    // More test cases
    public async Task CelsiusToFahrenheitAsync_ReturnsExpectedResult(decimal celsius, string expected)
    {
        var result = await CelsiusToFahrenheitAsync(celsius);

        Assert.That(result, Is.TypeOf<Something<string>>(), "Result should be a Something<string> type");
        var something = result as Something<string>;
        Assert.That(something?.Value, Is.EqualTo(expected));
    }
}