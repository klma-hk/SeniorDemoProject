```

BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.9974)
Intel Core Ultra X7 358H 3.69GHz, 1 CPU, 16 logical and 16 physical cores
Memory: 31.57 GB Total, 6.84 GB Available
.NET SDK 11.0.100
  [Host]     : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3


```
| Method               | DataSize  | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------- |---------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| **SimdChecksum**         | **1000000**   |    **53.45 μs** |  **1.020 μs** |  **1.174 μs** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| ParallelSimdChecksum | 1000000   |    46.45 μs |  0.445 μs |  0.416 μs |  0.87 |    0.02 | 0.2441 |    2610 B |          NA |
|                      |           |             |           |           |       |         |        |           |             |
| **SimdChecksum**         | **100000000** | **6,022.92 μs** | **57.889 μs** | **54.149 μs** |  **1.00** |    **0.00** |      **-** |         **-** |          **NA** |
| ParallelSimdChecksum | 100000000 |   835.58 μs |  3.642 μs |  3.041 μs |  0.14 |    0.00 | 0.9766 |    9718 B |          NA |
