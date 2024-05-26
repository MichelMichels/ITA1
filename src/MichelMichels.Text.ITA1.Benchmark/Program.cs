using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MichelMichels.Text;

[SimpleJob(RuntimeMoniker.Net80)]
[RPlotExporter]
public class Ita1EncodingBenchmark
{
    private Ita1Encoding ita1 = new();
    private string data = "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG";

    [Params(1000, 10000)]
    public int N;

    [Benchmark]
    public byte[] Encode() => ita1.GetBytes(data);
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Ita1EncodingBenchmark>();
    }
}

/*
 * Benchmark Release v1.1.0
 * 
 *   // * Summary *
 *   
 *   BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3593/23H2/2023Update/SunValley3)
 *   13th Gen Intel Core i7-13700H, 1 CPU, 20 logical and 14 physical cores
 *   .NET SDK 9.0.100-preview.4.24267.66
 *     [Host]   : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
 *     .NET 8.0 : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2
 *   Job=.NET 8.0  Runtime=.NET 8.0
 *   | Method | N     | Mean     | Error     | StdDev    |
 *   |------- |------ |---------:|----------:|----------:|
 *   | Encode | 1000  | 1.025 us | 0.0029 us | 0.0024 us |
 *   | Encode | 10000 | 1.014 us | 0.0072 us | 0.0064 us |
 *   
 *   // * Hints *
 *   
 *   Outliers
 *     Ita1EncodingBenchmark.Encode: .NET 8.0 -> 2 outliers were removed (1.04 us, 1.04 us)
 *     Ita1EncodingBenchmark.Encode: .NET 8.0 -> 1 outlier  was  removed, 2 outliers were detected (999.73 ns, 1.03 us)
 *     
 *   // * Legends *
 *   
 *     N      : Value of the 'N' parameter
 *     Mean   : Arithmetic mean of all measurements
 *     Error  : Half of 99.9% confidence interval
 *     StdDev : Standard deviation of all measurements
 *     1 us   : 1 Microsecond (0.000001 sec)
 *
 */