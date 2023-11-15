namespace BlazorAppMaybePoc.Client;

public static class TemperatureConversionService
{
    public static string FahrenheitToCelsius(decimal fahrenheit)
    {
        var celsius = (fahrenheit - 32) * 5 / 9;
        return $"{Math.Round(celsius, 2)}°C";
    }

    public static string CelsiusToFahrenheit(decimal celsius)
    {
        var fahrenheit = celsius * 9 / 5 + 32;
        return $"{Math.Round(fahrenheit, 2)}°F";
    }
}