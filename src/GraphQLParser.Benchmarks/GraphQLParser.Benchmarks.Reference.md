``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.14393.3243 (1607/AnniversaryUpdate/Redstone1)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=3515629 Hz, Resolution=284.4441 ns, Timer=TSC
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  DefaultJob : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT


```

Baseline:

| Method |  query |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated | Commit |
|------- |------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|-------:|
|  Parse | Params | 13.911 us | 0.2186 us | 0.1937 us | 4.6082 |      - |     - |  18.85 KB | 710e1b7 |
|  Parse | Schema | 30.629 us | 0.3194 us | 0.2987 us | 8.9111 | 0.0610 |     - |  36.49 KB | 710e1b7 |
|  Parse | Simple |  2.391 us | 0.0257 us | 0.0241 us | 0.8736 |      - |     - |   3.58 KB | 710e1b7 |

Working on cache:

|            Method |  query |      Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|--------:|-------:|------:|----------:|
|             **Parse** | **Params** | **14.729 us** | **0.2432 us** | **0.2275 us** |  **5.3711** | **0.0153** |     **-** |  **21.98 KB** |
| ParseCacheManaged | Params | 14.881 us | 0.1291 us | 0.1208 us |  4.6082 |      - |     - |  18.86 KB |
|  ParseCacheUnsafe | Params | 15.136 us | 0.2053 us | 0.1920 us |  4.6082 |      - |     - |  18.86 KB |
|             **Parse** | **Schema** | **32.302 us** | **0.3633 us** | **0.3398 us** | **10.1318** |      **-** |     **-** |  **41.52 KB** |
| ParseCacheManaged | Schema | 34.417 us | 0.2486 us | 0.2076 us |  8.3618 |      - |     - |  34.33 KB |
|  ParseCacheUnsafe | Schema | 34.713 us | 0.3510 us | 0.3112 us |  8.3618 |      - |     - |  34.33 KB |
|             **Parse** | **Simple** |  **2.564 us** | **0.0440 us** | **0.0390 us** |  **1.0109** |      **-** |     **-** |   **4.14 KB** |
| ParseCacheManaged | Simple |  2.638 us | 0.0319 us | 0.0299 us |  0.8392 |      - |     - |   3.44 KB |
|  ParseCacheUnsafe | Simple |  2.712 us | 0.0271 us | 0.0253 us |  0.8392 |      - |     - |   3.44 KB |

Make `LexerContext` struct and remove `IDisposable`:

|            Method |  query |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|             **Parse** | **Params** | **12.480 us** | **0.0555 us** | **0.0433 us** | **4.4098** |      **-** |     **-** |  **18.07 KB** |
| ParseCacheManaged | Params | 12.602 us | 0.0860 us | 0.0805 us | 3.6469 |      - |     - |  14.95 KB |
|  ParseCacheUnsafe | Params | 12.712 us | 0.0980 us | 0.0917 us | 3.6469 |      - |     - |  14.95 KB |
|             **Parse** | **Schema** | **28.297 us** | **0.1496 us** | **0.1400 us** | **8.6060** | **0.0305** |     **-** |  **35.23 KB** |
| ParseCacheManaged | Schema | 30.642 us | 0.6201 us | 0.9469 us | 6.8359 |      - |     - |  28.04 KB |
|  ParseCacheUnsafe | Schema | 30.130 us | 0.1893 us | 0.1771 us | 6.8359 |      - |     - |  28.04 KB |
|             **Parse** | **Simple** |  **2.186 us** | **0.0150 us** | **0.0140 us** | **0.8392** |      **-** |     **-** |   **3.44 KB** |
| ParseCacheManaged | Simple |  2.270 us | 0.0178 us | 0.0158 us | 0.6676 |      - |     - |   2.73 KB |
|  ParseCacheUnsafe | Simple |  2.237 us | 0.0284 us | 0.0266 us | 0.6676 |      - |     - |   2.73 KB |