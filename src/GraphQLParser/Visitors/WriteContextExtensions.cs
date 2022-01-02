using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLParser.Visitors
{
    /// <summary>
    /// Extension methods for writing into <see cref="TextWriter"/>.
    /// </summary>
    public static class WriteContextExtensions
    {
        private static readonly ROM _newLine = Environment.NewLine;

        /// <summary>
        /// Writes the specified <see cref="ROM"/> to the provided context
        /// using <see cref="CancellationToken"/> from it.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask Write<TContext>(this TContext context, ROM value)
            where TContext : IWriteContext
        {
            var task =
#if NETSTANDARD2_0
            // no cancellationToken support on netstandard2.0
            context.Writer.WriteAsync(value.ToString());
#elif NETSTANDARD2_1_OR_GREATER
            context.Writer.WriteAsync(value, context.CancellationToken);
#endif
            return new ValueTask(task);
        }

        /// <summary>
        /// Writes <see cref="Environment.NewLine"/> to the provided context
        /// using <see cref="CancellationToken"/> from it.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask WriteLine<TContext>(this TContext context)
            where TContext : IWriteContext
            => Write(context, _newLine);
    }
}
