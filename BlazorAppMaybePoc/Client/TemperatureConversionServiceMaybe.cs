using BlazorAppMaybePoc.Shared.Common;

namespace BlazorAppMaybePoc.Client;

public class MagicValueException : Exception
{
    public MagicValueException()
        : base("The temperature is the answer to life, the universe, and everything.")
    {
    }
}

public static class TemperatureConversionServiceMaybe
{
    private static decimal RoundBy2(decimal x)
    {
        return Math.Round(x, 2);
    }

    private static Task<decimal> RoundBy2Async(decimal x)
    {
        return Task.FromResult(Math.Round(x, 2));
    }

    public static Maybe<string> FahrenheitToCelsius(decimal fahrenheit)
    {
        return fahrenheit.ToMaybe()
            .Bind(DetectUniverseAnswer())
            .Bind(x => x - 32)
            .Bind(x => x * 5)
            .Bind(x => x / 9)
            .Bind(RoundBy2)
            .Bind(x => $"{x}°C");

        Func<decimal, decimal> DetectUniverseAnswer()
        {
            return x => x == 42 ? throw new MagicValueException() : x;
        }
    }

    public static async Task<Maybe<string>> CelsiusToFahrenheitAsync(decimal celsius)
    {
        return (await celsius.ToMaybe()
                .Bind(x => (x * 9 / 5 + 32))
                .BindAsync(RoundBy2Async))
            .Bind(x => $"{x}°F");
    }
}