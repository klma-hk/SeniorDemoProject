```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.9974)
Intel Core Ultra X7 358H 3.69GHz, 1 CPU, 16 logical and 16 physical cores
Memory: 31.57 GB Total, 6.84 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3


```
| Method                          | DataSize | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |--------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **HighPerformanceProcessorProcess** | **10000**    |  **3,502.2 ns** |  **62.57 ns** |  **58.53 ns** |  **1.03** |    **0.02** | **0.0114** |     **128 B** |        **1.07** |
| RegularProcessorProcess         | 10000    |  3,391.3 ns |  21.61 ns |  20.22 ns |  1.00 |    0.00 | 0.0114 |     120 B |        1.00 |
| DirectBufferProcessorProcess    | 10000    |  3,362.4 ns |  43.24 ns |  40.45 ns |  0.99 |    0.01 | 0.0114 |     128 B |        1.07 |
| SimdChecksum                    | 10000    |    551.0 ns |  10.48 ns |  12.06 ns |  0.16 |    0.00 |      - |         - |        0.00 |
|                                 |          |             |           |           |       |         |        |           |             |
| **HighPerformanceProcessorProcess** | **100000**   | **31,980.7 ns** | **205.83 ns** | **504.91 ns** |  **1.07** |    **0.02** |      **-** |     **128 B** |        **1.07** |
| RegularProcessorProcess         | 100000   | 30,027.4 ns | 109.52 ns | 102.45 ns |  1.00 |    0.00 |      - |     120 B |        1.00 |
| DirectBufferProcessorProcess    | 100000   | 30,053.3 ns |  58.57 ns |  51.92 ns |  1.00 |    0.00 |      - |     128 B |        1.07 |
| SimdChecksum                    | 100000   |  5,420.7 ns |  99.54 ns |  93.11 ns |  0.18 |    0.00 |      - |         - |        0.00 |
