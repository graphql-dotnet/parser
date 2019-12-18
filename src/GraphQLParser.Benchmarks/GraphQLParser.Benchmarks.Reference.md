``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.14393.3243 (1607/AnniversaryUpdate/Redstone1)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=3515629 Hz, Resolution=284.4441 ns, Timer=TSC
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  DefaultJob : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT


```
| Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |  Commit |
|------- |---------:|----------:|----------:|-------:|------:|------:|----------:|--------:|
|  Parse | 2.556 us | 0.0430 us | 0.0402 us | 0.8736 |     - |     - |   3.58 KB | 8fcf2c2 |
