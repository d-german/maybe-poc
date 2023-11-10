using BlazorAppMaybePoc.Shared.Common;

namespace BlazorAppMaybePoc.Client;

public class UniverseAnswerTemperatureException : Exception
{
    public UniverseAnswerTemperatureException()
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
            .Bind(x => $"{x}°C")
            .OnSomething(Console.WriteLine) // This is a side effect demonstrating the use of logging in a Maybe pipeline
            .OnNothing(() => Console.WriteLine("Nothing"))
            .OnError(e => Console.WriteLine(e.Message));

        Func<decimal, decimal> DetectUniverseAnswer()
        {
            return x => x == 42 ? throw new UniverseAnswerTemperatureException() : x;
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