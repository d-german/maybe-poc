namespace BlazorAppMaybePoc.Client;

public class TemperatureConversionService
{
    public decimal FahrenheitToCelsius(decimal fahrenheit)
    {
        return Math.Round((fahrenheit - 32) * 5 / 9, 2);
    }

    public decimal CelsiusToFahrenheit(decimal celsius)
    {
        return Math.Round(celsius * 9 / 5 + 32, 2);
    }
}