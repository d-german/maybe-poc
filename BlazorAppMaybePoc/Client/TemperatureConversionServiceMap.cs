using BlazorAppMaybePoc.Shared.Functional;

namespace BlazorAppMaybePoc.Client;

public static class TemperatureConversionServiceMap
{
    public static string FahrenheitToCelsius(decimal fahrenheit)
    {
        return fahrenheit.Map(x => x - 32)
            .Map(x => x * 5)
            .Map(x => x / 9)
            .Map(x => Math.Round(x, 2))
            .Map(x => x + "°C");
    }

    public static string CelsiusToFahrenheit(decimal celsius)
    {
        return celsius.Map(x => x * 9)
            .Map(x => x / 5)
            .Map(x => x + 32)
            .Map(x => Math.Round(x, 2))
            .Map(x => x + "°F");
    }
}