``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.14393.3243 (1607/AnniversaryUpdate/Redstone1)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=3515629 Hz, Resolution=284.4441 ns, Timer=TSC
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  DefaultJob : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT


```
| Method |  query |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated | Commit |
|------- |------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|-------:|
|  Parse | Params | 13.911 us | 0.2186 us | 0.1937 us | 4.6082 |      - |     - |  18.85 KB | 710e1b7 |
|  Parse | Schema | 30.629 us | 0.3194 us | 0.2987 us | 8.9111 | 0.0610 |     - |  36.49 KB | 710e1b7 |
|  Parse | Simple |  2.391 us | 0.0257 us | 0.0241 us | 0.8736 |      - |     - |   3.58 KB | 710e1b7 |
