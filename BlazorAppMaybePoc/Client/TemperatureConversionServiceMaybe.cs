using BlazorAppMaybePoc.Shared.Common;

namespace BlazorAppMaybePoc.Client;

public static class TemperatureConversionServiceMaybe
{
    private static decimal RoundBy2(decimal x)
    {
        return Math.Round(x, 2);
    }

    public static Maybe<string> FahrenheitToCelsius(decimal fahrenheit)
    {
        var fahrenheitToCelsius = fahrenheit.ToMaybe();
        var minus32 = fahrenheitToCelsius.Bind(x => x - 32);
        var times5 = minus32.Bind(x => x * 5);
        var dividedBy9 = times5.Bind(x => x / 9);
        var rounded = dividedBy9.Bind(RoundBy2);
        var celsius = rounded.Bind(x => $"{x}°C");

        return celsius;
    }

    public static Maybe<string> CelsiusToFahrenheit(decimal celsius)
    {
        return celsius.ToMaybe()
            .Bind(x => (x * 9 / 5 + 32))
            .Bind(RoundBy2)
            .Bind(x => $"{x}°F");
    }
}