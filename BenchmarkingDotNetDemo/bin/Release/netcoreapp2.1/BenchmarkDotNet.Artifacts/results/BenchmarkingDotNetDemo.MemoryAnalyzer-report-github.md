``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i7-4790 CPU 3.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=3507568 Hz, Resolution=285.0978 ns, Timer=TSC
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT


```
|  Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|-------- |----------:|----------:|----------:|-------:|----------:|
|   Boxes | 134.24 ns | 0.4971 ns | 0.3881 ns | 0.0360 |     152 B |
| UnBoxes |  28.68 ns | 0.6309 ns | 0.5593 ns | 0.0228 |      96 B |
