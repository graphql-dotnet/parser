using System;

namespace GraphQLParser;

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
        // fast check in case of memory is backed by the same string object
        //public bool Equals(ReadOnlyMemory<T> other)
        //{
        //    if (_object == other._object && _index == other._index)
        //    {
        //        return _length == other._length;
        //    }

        //    return false;
        //}
        if (_memory.Equals(other._memory))
            return true;

        // then check byte by byte
        if (_memory.Length != other._memory.Length)
            return false;

        return _memory.Span.SequenceEqual(other._memory.Span);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => _memory.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => _memory.ToString();

    /// <summary>
    /// Implicitly casts ReadOnlyMemory&lt;char&gt; to <see cref="ROM"/>.
    /// </summary>
    public static implicit operator ROM(ReadOnlyMemory<char> memory) => new(memory);

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
    public static implicit operator ROM(Memory<char> memory) => new(memory);

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
