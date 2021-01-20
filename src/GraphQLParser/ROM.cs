using System;

namespace GraphQLParser
{
    /// <summary>
    /// A wrapper around ReadOnlyMemory{char} allowing you to use simple syntax when working with it.
    /// <br/>
    /// Marshal.SizeOf(ROM) = Marshal.SizeOf(ReadOnlyMemory{char}) = 16
    /// </summary>
    public readonly struct ROM : IEquatable<ROM>
    {
        private readonly ReadOnlyMemory<char> _memory;

        /// <summary>
        /// Wraps an instance of <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        public ROM(ReadOnlyMemory<char> memory)
        {
            _memory = memory;
        }

        /// <inheritdoc cref="ReadOnlyMemory{T}.Length"/>
        public int Length => _memory.Length;

        /// <inheritdoc cref="ReadOnlyMemory{T}.IsEmpty"/>
        public bool IsEmpty => _memory.IsEmpty;

        /// <inheritdoc cref="ReadOnlyMemory{T}.Span"/>
        public ReadOnlySpan<char> Span => _memory.Span;

        /// <inheritdoc cref="ReadOnlyMemory{T}.Slice(int)"/>
        public ROM Slice(int start) => _memory.Slice(start);

        /// <inheritdoc cref="ReadOnlyMemory{T}.Slice(int, int)"/>
        public ROM Slice(int start, int length) => _memory.Slice(start, length);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ROM rom && Equals(rom);

        /// <inheritdoc/>
        public bool Equals(ROM other)
        {
            if (_memory.Length != other._memory.Length)
                return false;

            return _memory.Span.SequenceEqual(other._memory.Span);
        }

        /// <inheritdoc/>
        public override int GetHashCode() //TODO: find a better implementation
        {
            if (_memory.Length == 0)
                return 0;

            int num1 = 5381;
            int num2 = num1;
            int num3;
            int end = _memory.Length - 1;
            var span = _memory.Span;

            for (int i = 0; i <= end; i += 2)
            {
                num3 = span[i];
                num1 = (num1 << 5) + num1 ^ num3;
                if (i == end)
                    break;
                int num4 = span[i + 1];
                //if (num4 != 0)
                num2 = (num2 << 5) + num2 ^ num4;
                //else
                //    break;
            }

            return num1 + num2 * 1566083941;
        }

        /// <inheritdoc/>
        public override string ToString() => _memory.ToString();

        /// <summary>
        /// Implicitly casts ReadOnlyMemory&lt;char&gt; to <see cref="ROM"/>.
        /// </summary>
        public static implicit operator ROM(ReadOnlyMemory<char> memory) => new ROM(memory);

        /// <summary>
        /// Implicitly casts <see cref="ROM"/> to ReadOnlyMemory&lt;char&gt;.
        /// </summary>
        public static implicit operator ReadOnlyMemory<char>(ROM rom) => rom._memory;

        /// <summary>
        /// Implicitly casts <see cref="ROM"/> to ReadOnlySpan&lt;char&gt;.
        /// </summary>
        public static implicit operator ReadOnlySpan<char>(ROM rom) => rom._memory.Span;

        /// <summary>
        /// Implicitly casts Memory&lt;char&gt; to <see cref="ROM"/>.
        /// </summary>
        public static implicit operator ROM(Memory<char> memory) => new ROM(memory);

        /// <summary>
        /// Implicitly casts string to <see cref="ROM"/>.
        /// </summary>
        public static implicit operator ROM(string s) => s.AsMemory();

        /// <summary>
        /// Explicitly casts <see cref="ROM"/> to string.
        /// </summary>
        public static explicit operator string(ROM rom) => rom.ToString();

        /// <summary>
        /// Implicitly casts array of chars to <see cref="ROM"/>.
        /// </summary>
        public static implicit operator ROM(char[] array) => new ReadOnlyMemory<char>(array);

        /// <summary>
        /// Checks two ROMs for equality. The check is based on the actual contents of the two chunks of memory.
        /// </summary>
        public static bool operator ==(ROM rom1, ROM rom2) => rom1.Equals(rom2);

        /// <summary>
        /// Checks two ROMs for inequality. The check is based on the actual contents of the two chunks of memory.
        /// </summary>
        public static bool operator !=(ROM rom1, ROM rom2) => !rom1.Equals(rom2);

        /// <summary>
        /// Checks ROM and string for equality. The check is based on the actual contents of the two chunks of memory.
        /// </summary>
        public static bool operator ==(ROM rom, string s) => rom._memory.Span.SequenceEqual(s.AsSpan());

        /// <summary>
        /// Checks ROM and string for inequality. The check is based on the actual contents of the two chunks of memory.
        /// </summary>
        public static bool operator !=(ROM rom, string s) => !rom._memory.Span.SequenceEqual(s.AsSpan());

        /// <summary>
        /// Checks string and ROM and for equality. The check is based on the actual contents of the two chunks of memory.
        /// </summary>
        public static bool operator ==(string s, ROM rom) => rom == s;

        /// <summary>
        /// Checks string and ROM and for inequality. The check is based on the actual contents of the two chunks of memory.
        /// </summary>
        public static bool operator !=(string s, ROM rom) => rom != s;
    }
}
