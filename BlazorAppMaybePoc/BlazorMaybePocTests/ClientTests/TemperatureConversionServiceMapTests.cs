using BlazorAppMaybePoc.Client;

namespace BlazorMaybePocTests.ClientTests;

[TestFixture]
public class TemperatureConversionServiceMapTests
{
    [TestCase(32.0, "0°C")]
    [TestCase(212.0, "100°C")]
    [TestCase(0.0, "-17.78°C")]
    [TestCase(100.0, "37.78°C")]
    // Add more test cases as needed
    public void FahrenheitToCelsius_ReturnsExpectedResult(double fahrenheit, string expectedCelsius)
    {
        var result = TemperatureConversionServiceMap.FahrenheitToCelsius((decimal)fahrenheit);
        Assert.That(result, Is.EqualTo(expectedCelsius));
    }

    [TestCase(0.0, "32°F")]
    [TestCase(100.0, "212°F")]
    [TestCase(-17.78, "0.00°F")]
    [TestCase(37.78, "100.00°F")]
    // Add more test cases as needed
    public void CelsiusToFahrenheit_ReturnsExpectedResult(double celsius, string expectedFahrenheit)
    {
        var result = TemperatureConversionServiceMap.CelsiusToFahrenheit((decimal)celsius);
        Assert.That(result, Is.EqualTo(expectedFahrenheit));
    }
}