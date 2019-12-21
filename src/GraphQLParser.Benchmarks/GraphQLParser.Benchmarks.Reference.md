``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.14393.3243 (1607/AnniversaryUpdate/Redstone1)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=3515629 Hz, Resolution=284.4441 ns, Timer=TSC
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  DefaultJob : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT


```

Baseline (B) to current (C) comparison without cache:

| Method |  query |  Mean (B) |  Mean (C) | Ratio | Allocated (B) | Allocated (C) | Ratio |
|------- |------- |----------:|----------:|------:|--------------:|--------------:|------:|
|  Parse | Params | 13.911 us | 10.235 us |  0.74 |      18.85 KB |       8.59 KB |  0.46 |
|  Parse | Schema | 30.629 us | 23.523 us |  0.77 |      36.49 KB |      19.09 KB |  0.52 |
|  Parse | Simple |  2.391 us |  1.754 us |  0.73 |       3.58 KB |       1.63 KB |  0.46 |

Baseline (B) to current (C) comparison with cache:

| Method |  query |  Mean (B) |  Mean (C) | Ratio | Allocated (B) | Allocated (C) | Ratio |
|------- |------- |----------:|----------:|------:|--------------:|--------------:|------:|
|  Parse | Params | 13.911 us | 10.743 us |  0.77 |      18.85 KB |       7.82 KB |  0.41 |
|  Parse | Schema | 30.629 us | 26.048 us |  0.85 |      36.49 KB |      15.66 KB |  0.43 |
|  Parse | Simple |  2.391 us |  1.860 us |  0.78 |       3.58 KB |       1.35 KB |  0.38 |

___

Baseline (710e1b7):

| Method |  query |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------- |------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|  Parse | Params | 13.911 us | 0.2186 us | 0.1937 us | 4.6082 |      - |     - |  18.85 KB |
|  Parse | Schema | 30.629 us | 0.3194 us | 0.2987 us | 8.9111 | 0.0610 |     - |  36.49 KB |
|  Parse | Simple |  2.391 us | 0.0257 us | 0.0241 us | 0.8736 |      - |     - |   3.58 KB |

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

Make `Stack<GraphQLComment>` allocation lazy:

|            Method |  query |      Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|-------:|-------:|------:|----------:|
|             **Parse** | **Params** | **12.511 us** | **0.1122 us** | **0.1050 us** | **4.4098** |      **-** |     **-** |  **18.04 KB** |
| ParseCacheManaged | Params | 12.579 us | 0.0485 us | 0.0405 us | 3.6469 |      - |     - |  14.92 KB |
|  ParseCacheUnsafe | Params | 12.513 us | 0.0434 us | 0.0406 us | 3.6469 |      - |     - |  14.92 KB |
|             **Parse** | **Schema** | **28.169 us** | **0.1233 us** | **0.1093 us** | **8.6060** | **0.0916** |     **-** |   **35.2 KB** |
| ParseCacheManaged | Schema | 30.041 us | 0.1271 us | 0.1127 us | 6.8359 |      - |     - |  28.01 KB |
|  ParseCacheUnsafe | Schema | 30.351 us | 0.1308 us | 0.1224 us | 6.8359 |      - |     - |  28.01 KB |
|             **Parse** | **Simple** |  **2.195 us** | **0.0532 us** | **0.0653 us** | **0.8316** |      **-** |     **-** |   **3.41 KB** |
| ParseCacheManaged | Simple |  2.282 us | 0.0208 us | 0.0195 us | 0.6599 |      - |     - |    2.7 KB |
|  ParseCacheUnsafe | Simple |  2.304 us | 0.0164 us | 0.0154 us | 0.6599 |      - |     - |    2.7 KB |

Avoid `Func<T>` closure allocation:

|            Method |  query |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|------:|----------:|
|             **Parse** | **Params** | **12.651 us** | **0.0699 us** | **0.0620 us** |  **1.00** |    **0.00** | **4.2267** | **0.0153** |     **-** |  **17.29 KB** |
| ParseCacheManaged | Params | 12.683 us | 0.0965 us | 0.0754 us |  1.00 |    0.01 | 3.4637 |      - |     - |  14.17 KB |
|  ParseCacheUnsafe | Params | 12.547 us | 0.0629 us | 0.0525 us |  0.99 |    0.00 | 3.4637 |      - |     - |  14.17 KB |
|                   |        |           |           |           |       |         |        |        |       |           |
|             **Parse** | **Schema** | **29.192 us** | **0.3598 us** | **0.3366 us** |  **1.00** |    **0.00** | **8.1482** | **0.0305** |     **-** |  **33.33 KB** |
| ParseCacheManaged | Schema | 30.558 us | 0.2245 us | 0.1990 us |  1.05 |    0.01 | 6.3782 | 0.0610 |     - |  26.13 KB |
|  ParseCacheUnsafe | Schema | 31.056 us | 0.2093 us | 0.1958 us |  1.06 |    0.02 | 6.3477 | 0.1221 |     - |  26.13 KB |
|                   |        |           |           |           |       |         |        |        |       |           |
|             **Parse** | **Simple** |  **2.201 us** | **0.0306 us** | **0.0286 us** |  **1.00** |    **0.00** | **0.7858** |      **-** |     **-** |   **3.22 KB** |
| ParseCacheManaged | Simple |  2.302 us | 0.0171 us | 0.0160 us |  1.05 |    0.02 | 0.6142 |      - |     - |   2.52 KB |
|  ParseCacheUnsafe | Simple |  2.286 us | 0.0368 us | 0.0344 us |  1.04 |    0.01 | 0.6142 |      - |     - |   2.52 KB |

Make `GraphQLLocation` struct:

|            Method |  query |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|------:|--------:|-------:|------:|------:|----------:|
|             **Parse** | **Params** | **11.901 us** | **0.1156 us** | **0.1082 us** |  **1.00** |    **0.00** | **3.1433** |     **-** |     **-** |  **12.86 KB** |
| ParseCacheManaged | Params | 12.567 us | 0.1518 us | 0.1346 us |  1.06 |    0.01 | 2.9449 |     - |     - |  12.09 KB |
|  ParseCacheUnsafe | Params | 12.410 us | 0.0778 us | 0.0728 us |  1.04 |    0.01 | 2.9449 |     - |     - |  12.09 KB |
|                   |        |           |           |           |       |         |        |       |       |           |
|             **Parse** | **Schema** | **26.922 us** | **0.2194 us** | **0.2052 us** |  **1.00** |    **0.00** | **6.1340** |     **-** |     **-** |  **25.08 KB** |
| ParseCacheManaged | Schema | 29.734 us | 0.3812 us | 0.3566 us |  1.10 |    0.02 | 5.2795 |     - |     - |  21.66 KB |
|  ParseCacheUnsafe | Schema | 29.102 us | 0.4225 us | 0.3952 us |  1.08 |    0.02 | 5.2795 |     - |     - |  21.66 KB |
|                   |        |           |           |           |       |         |        |       |       |           |
|             **Parse** | **Simple** |  **2.062 us** | **0.0275 us** | **0.0244 us** |  **1.00** |    **0.00** | **0.5798** |     **-** |     **-** |   **2.38 KB** |
| ParseCacheManaged | Simple |  2.125 us | 0.0188 us | 0.0147 us |  1.03 |    0.01 | 0.5112 |     - |     - |   2.09 KB |
|  ParseCacheUnsafe | Simple |  2.229 us | 0.0446 us | 0.0610 us |  1.09 |    0.03 | 0.5112 |     - |     - |   2.09 KB |

Make `Token` struct:

|            Method |  query |      Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|------:|-------:|------:|------:|----------:|
|             **Parse** | **Params** | **11.358 us** | **0.0581 us** | **0.0515 us** |  **1.00** | **2.1820** |     **-** |     **-** |   **8.97 KB** |
| ParseCacheManaged | Params | 11.988 us | 0.0729 us | 0.0646 us |  1.06 | 1.9989 |     - |     - |    8.2 KB |
|  ParseCacheUnsafe | Params | 11.725 us | 0.0633 us | 0.0561 us |  1.03 | 1.9989 |     - |     - |    8.2 KB |
|                   |        |           |           |           |       |        |       |       |           |
|             **Parse** | **Schema** | **26.159 us** | **0.0621 us** | **0.0581 us** |  **1.00** | **4.5776** |     **-** |     **-** |   **18.8 KB** |
| ParseCacheManaged | Schema | 28.185 us | 0.1395 us | 0.1305 us |  1.08 | 3.7537 |     - |     - |  15.38 KB |
|  ParseCacheUnsafe | Schema | 28.447 us | 0.1315 us | 0.1166 us |  1.09 | 3.7537 |     - |     - |  15.38 KB |
|                   |        |           |           |           |       |        |       |       |           |
|             **Parse** | **Simple** |  **1.975 us** | **0.0068 us** | **0.0053 us** |  **1.00** | **0.4120** |     **-** |     **-** |   **1.69 KB** |
| ParseCacheManaged | Simple |  2.081 us | 0.0172 us | 0.0161 us |  1.05 | 0.3433 |     - |     - |   1.41 KB |
|  ParseCacheUnsafe | Simple |  2.087 us | 0.0143 us | 0.0127 us |  1.06 | 0.3433 |     - |     - |   1.41 KB |

Make `ParserContext` struct and avoid closure allocation in `ParseDefinitionsIfNotEOF` (yield):

|            Method |  query |      Mean |     Error |    StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|------:|-------:|-------:|------:|----------:|
|             **Parse** | **Params** | **11.378 us** | **0.1056 us** | **0.0936 us** |  **1.00** | **2.1667** | **0.0153** |     **-** |   **8.87 KB** |
| ParseCacheManaged | Params | 11.619 us | 0.0655 us | 0.0547 us |  1.02 | 1.9684 |      - |     - |   8.09 KB |
|  ParseCacheUnsafe | Params | 11.732 us | 0.1053 us | 0.0985 us |  1.03 | 1.9684 |      - |     - |   8.09 KB |
|                   |        |           |           |           |       |        |        |       |           |
|             **Parse** | **Schema** | **25.675 us** | **0.1323 us** | **0.1238 us** |  **1.00** | **4.5776** |      **-** |     **-** |   **18.7 KB** |
| ParseCacheManaged | Schema | 27.889 us | 0.1265 us | 0.1121 us |  1.09 | 3.7231 |      - |     - |  15.28 KB |
|  ParseCacheUnsafe | Schema | 27.885 us | 0.1956 us | 0.1830 us |  1.09 | 3.7231 |      - |     - |  15.28 KB |
|                   |        |           |           |           |       |        |        |       |           |
|             **Parse** | **Simple** |  **1.917 us** | **0.0096 us** | **0.0090 us** |  **1.00** | **0.3853** |      **-** |     **-** |   **1.59 KB** |
| ParseCacheManaged | Simple |  2.024 us | 0.0142 us | 0.0125 us |  1.06 | 0.3166 |      - |     - |    1.3 KB |
|  ParseCacheUnsafe | Simple |  2.015 us | 0.0199 us | 0.0187 us |  1.05 | 0.3166 |      - |     - |    1.3 KB |

Change all `IEnumerable<T>` to `List<T>` to avoid `List<T>.Enumerator` allocations on caller side:

|            Method |  query |      Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |------- |----------:|----------:|----------:|------:|-------:|------:|------:|----------:|
|             **Parse** | **Params** | **10.235 us** | **0.0286 us** | **0.0267 us** |  **1.00** | **2.0905** |     **-** |     **-** |   **8.59 KB** |
| ParseCacheManaged | Params | 10.743 us | 0.0397 us | 0.0371 us |  1.05 | 1.9073 |     - |     - |   7.82 KB |
|  ParseCacheUnsafe | Params | 10.802 us | 0.1091 us | 0.0911 us |  1.06 | 1.9073 |     - |     - |   7.82 KB |
|                   |        |           |           |           |       |        |       |       |           |
|             **Parse** | **Schema** | **23.523 us** | **0.1073 us** | **0.0896 us** |  **1.00** | **4.6692** |     **-** |     **-** |  **19.09 KB** |
| ParseCacheManaged | Schema | 26.048 us | 0.0814 us | 0.0721 us |  1.11 | 3.8147 |     - |     - |  15.66 KB |
|  ParseCacheUnsafe | Schema | 25.921 us | 0.1116 us | 0.0989 us |  1.10 | 3.8147 |     - |     - |  15.66 KB |
|                   |        |           |           |           |       |        |       |       |           |
|             **Parse** | **Simple** |  **1.754 us** | **0.0140 us** | **0.0124 us** |  **1.00** | **0.3986** |     **-** |     **-** |   **1.63 KB** |
| ParseCacheManaged | Simple |  1.860 us | 0.0086 us | 0.0076 us |  1.06 | 0.3300 |     - |     - |   1.35 KB |
|  ParseCacheUnsafe | Simple |  1.850 us | 0.0036 us | 0.0030 us |  1.06 | 0.3300 |     - |     - |   1.35 KB |