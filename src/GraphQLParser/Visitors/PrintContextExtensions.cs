using System.Runtime.CompilerServices;

namespace GraphQLParser.Visitors;

/// <summary>
/// Extension methods for writing into <see cref="TextWriter"/>.
/// </summary>
public static class PrintContextExtensions
{
    private static readonly ROM _newLine = Environment.NewLine;
    private static readonly ROM _newLineDoubled = Environment.NewLine + Environment.NewLine;

    /// <summary>
    /// Writes the specified <see cref="ROM"/> to the provided context
    /// using <see cref="CancellationToken"/> from it.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask WriteAsync<TContext>(this TContext context, ROM value)
        where TContext : IPrintContext
    {
        var task =
#if NETSTANDARD2_0
        // no cancellationToken support on netstandard2.0
        context.Writer.WriteAsync(value.ToString()); //ISSUE: allocation - either WriteAsync(value.ToString()) or Write(char value) in a loop
#elif NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
            context.Writer.WriteAsync(value, context.CancellationToken);
#endif
        return new ValueTask(task);
    }

    /// <summary>
    /// Writes <see cref="Environment.NewLine"/> to the provided context
    /// using <see cref="CancellationToken"/> from it.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask WriteLineAsync<TContext>(this TContext context)
        where TContext : IPrintContext
        => WriteAsync(context, _newLine);

    /// <summary>
    /// Writes doubled <see cref="Environment.NewLine"/> to the provided context
    /// using <see cref="CancellationToken"/> from it.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask WriteDoubleLineAsync<TContext>(this TContext context)
        where TContext : IPrintContext
        => WriteAsync(context, _newLineDoubled);
}
