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
|                              Method |      Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------ |----------:|---------:|---------:|-------:|------:|------:|----------:|
| DirectCelsiusToFahrenheitConversion |  60.25 ns | 1.142 ns | 1.068 ns | 0.0024 |     - |     - |      32 B |
|          MaybeHeapAllocWithDelegate | 104.01 ns | 2.069 ns | 2.383 ns | 0.0165 |     - |     - |     216 B |
|           MaybeStackAllocWithLambda |  96.56 ns | 1.722 ns | 1.611 ns | 0.0116 |     - |     - |     152 B |
|         ResultHeapAllocWithDelegate | 101.70 ns | 0.888 ns | 0.787 ns | 0.0073 |     - |     - |      96 B |
|          ResultStackAllocWithLambda |  96.04 ns | 1.795 ns | 2.137 ns | 0.0024 |     - |     - |      32 B |
*/