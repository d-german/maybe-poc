using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BlazorAppMaybePoc.Client;

namespace benchmark;

[MemoryDiagnoser]
public class MaybeBenchmark
{
    [Benchmark]
    public void DirectCelsiusToFahrenheitConversion()
    {
        _ = TemperatureConversionService.DirectCelsiusToFahrenheitConversion(100);
    }

    [Benchmark]
    public void MaybeHeapAllocWithDelegate()
    {
        _ = TemperatureConversionServiceMonad.CelsiusToFahrenheitWithDelegateMaybe(100);
    }

    [Benchmark]
    public void MaybeStackAllocWithLambda()
    {
        _ = TemperatureConversionServiceMonad.CelsiusToFahrenheitWithoutDelegateMaybe(100);
    }

    [Benchmark]
    public void ResultHeapAllocWithDelegate()
    {
        _ = TemperatureConversionServiceMonad.CelsiusToFahrenheitWithDelegateResult(100);
    }

    [Benchmark]
    public void ResultStackAllocWithLambda()
    {
        _ = TemperatureConversionServiceMonad.CelsiusToFahrenheitWithoutDelegateResult(100);
    }
}

static class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<MaybeBenchmark>();
    }
}

/*
|  Method |     Mean |   Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------- |---------:|--------:|--------:|-------:|------:|------:|----------:|
|   Maybe | 160.3 ns | 0.93 ns | 0.78 ns | 0.0165 |     - |     - |     216 B |
| NoMaybe | 101.9 ns | 0.16 ns | 0.13 ns | 0.0024 |     - |     - |      32 B |

The difference of 60 nanoseconds is equivalent to 0.00006 milliseconds (or 60 microseconds).

*/