using System;
using System.Buffers;
using System.Collections.Generic;

namespace GraphQLParser;

/// <summary>
/// Extension methods for parsing GraphQL documents.
/// </summary>
internal static class ParserExtensions
{
    internal static (IMemoryOwner<char> owner, ROM result) Concat(this List<ROM> parts)
    {
        var newLine = Environment.NewLine.AsSpan();

        int length = 0;
        foreach (var part in parts)
            length += part.Length;

        length += newLine.Length * (parts.Count - 1);

        var owner = MemoryPool<char>.Shared.Rent(length);
        var memory = owner.Memory.Slice(0, length); // since length of the pooled array very likely may be greater 
        var destination = memory.Span;

        for (int i = 0; i < parts.Count; ++i)
        {
            parts[i].Span.CopyTo(destination);
            destination = destination.Slice(parts[i].Length);
            if (i < parts.Count - 1)
            {
                newLine.CopyTo(destination);
                destination = destination.Slice(newLine.Length);
            }
        }

        return (owner, memory);
    }
}
