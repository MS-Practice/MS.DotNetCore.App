``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=3507568 Hz, Resolution=285.0978 ns, Timer=TSC
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT


```
| Method |       Needle |             Mean |           Error |          StdDev |
|------- |------------- |-----------------:|----------------:|----------------:|
| **Single** |    **EndNeedle** | **14,258,393.58 ns** |  **83,886.4800 ns** |  **74,363.1722 ns** |
|  First |    EndNeedle | 13,461,887.63 ns |  68,312.3569 ns |  60,557.1191 ns |
| **Single** | **MiddleNeedle** | **14,151,675.26 ns** |  **59,186.7848 ns** |  **52,467.5379 ns** |
|  First | MiddleNeedle |  6,776,449.31 ns | 124,052.1560 ns |  96,851.7552 ns |
| **Single** |  **StartNeedle** | **14,143,560.61 ns** | **282,046.2368 ns** | **277,007.0910 ns** |
|  First |  StartNeedle |         48.92 ns |       0.2591 ns |       0.2164 ns |
