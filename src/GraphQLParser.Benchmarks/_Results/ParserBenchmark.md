``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.17134.1130 (1803/April2018Update/Redstone4)
Intel Core i5-6200U CPU 2.30GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
Frequency=2343750 Hz, Resolution=426.6667 ns, Timer=TSC
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 32bit RyuJIT
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 32bit RyuJIT


```

Before optimizations. Token/GraphQLLocation/LexerContext are classes:

| Method |     Mean |     Error |    StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------- |---------:|----------:|----------:|------------:|------------:|------------:|--------------------:|
|  Parse | 3.696 us | 0.0736 us | 0.0876 us |      1.3046 |           - |           - |                2 KB |


After optimizations.Token/GraphQLLocation/LexerContext are structs:

| Method |     Mean |     Error |    StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------- |---------:|----------:|----------:|------------:|------------:|------------:|--------------------:|
|  Parse | 3.011 us | 0.0852 us | 0.1248 us |      0.7019 |           - |           - |             1.08 KB |


