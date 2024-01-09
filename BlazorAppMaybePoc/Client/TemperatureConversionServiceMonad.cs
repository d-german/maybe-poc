using BlazorAppMaybePoc.Shared.Functional;
using DotNext;
using DotNext.Runtime.CompilerServices;

namespace BlazorAppMaybePoc.Client;

public class UniverseAnswerTemperatureException : Exception
{
    public UniverseAnswerTemperatureException()
        : base("The temperature is the answer to life, the universe, and everything.")
    {
    }
}

public struct RoundBy2Supplier : ISupplier<decimal, decimal>
{
    public decimal Invoke(decimal arg) => Math.Round(arg, 2);
    Func<decimal, decimal> IFunctional<Func<decimal, decimal>>.ToDelegate() => Invoke;
}

public static class TemperatureConversionServiceMonad
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
            .OnSomething(Console.WriteLine)
            .OnNothing(() => Console.WriteLine("Nothing"))
            .OnError(e => Console.WriteLine(e.Message));

        Func<decimal, decimal> DetectUniverseAnswer()
        {
            return x => x == 42 ? throw new UniverseAnswerTemperatureException() : x;
        }
    }

    public static Maybe<string> CelsiusToFahrenheitWithDelegateMaybe(decimal celsius)
    {
        return celsius.ToMaybe()
            .Bind(x => x * 9 / 5 + 32)
            .Bind(RoundBy2)
            .Bind(x => $"{x}°F");
    }

    public static Maybe<string> CelsiusToFahrenheitWithoutDelegateMaybe(decimal celsius)
    {
        return celsius.ToMaybe()
            .Bind(x => x * 9 / 5 + 32)
            .Bind(x => Math.Round(x, 2))
            .Bind(x => $"{x}°F");
    }

    public static Result<string> CelsiusToFahrenheitWithDelegateResult(decimal celsius)
    {
        return Result.FromValue(celsius)
            .Convert(x => x * 9 / 5 + 32)
            .Convert(RoundBy2)
            .Convert(x => $"{x}°F");
    }

    public static Result<string> CelsiusToFahrenheitWithoutDelegateResult(decimal celsius)
    {
        return Result.FromValue(celsius)
            .Convert(x => x * 9 / 5 + 32)
            .Convert(x => Math.Round(x, 2))
            .Convert(x => $"{x}°F");
    }

    public static async Task<Maybe<string>> CelsiusToFahrenheitAsync(decimal celsius)
    {
        return (await celsius.ToMaybe().Bind(x => (x * 9 / 5 + 32))
                .BindAsync(RoundBy2Async))
            .Bind(x => $"{x}°F");
    }
}