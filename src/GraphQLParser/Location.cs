namespace GraphQLParser;

/// <summary>
/// Provides the ability to decode a linear character position into a line and column number.
/// </summary>
public readonly struct Location : IEquatable<Location>
{
    /// <summary>
    /// Initializes a new instance with the specified parameters.
    /// </summary>
    public Location(int line, int column)
    {
        Line = line;
        Column = column;
    }

    /// <summary>
    /// The line number on which the character is located.
    /// </summary>
    public int Line { get; }

    /// <summary>
    /// The column number on which the character is located.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Creates location from a given sequence of characters and a linear character position.
    /// </summary>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="position">Linear character position in the <paramref name="source"/>.</param>
    public static Location FromLinearPosition(ROM source, int position)
    {
        // handle index overflow (EOF)
        int overflowDelta = 0;
        if (position >= source.Length)
        {
            overflowDelta = position - source.Length + 1; // equals 1 in case of EOF
            position = source.Length - 1;
        }

        int line = 1;
        int column = 0;

        var span = source.Span;

        for (int i = 0; i <= position; ++i)
        {
            // each case should either
            // 1) increment Column by 1
            // or
            // 2) increment Line by 1 and reset Column to 0
            switch (span[i])
            {
                case '\n':
                    if (i == position) // \n at the end
                    {
                        ++column;
                    }
                    else
                    {
                        ++line;
                        column = 0;
                    }
                    break;

                case '\r':
                    if (i == position) // \r at the end
                    {
                        ++column;
                    }
                    else
                    {
                        char next = span[i + 1];
                        if (next == '\n') // \r\n at the end
                        {
                            ++column;
                        }
                        else
                        {
                            ++line;
                            column = 0;
                        }
                    }
                    break;

                default:
                    ++column;
                    break;
            }
        }

        return new Location(line, column + overflowDelta);
    }

    /// <inheritdoc/>
    public bool Equals(Location other) => Line == other.Line && Column == other.Column;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Location l && Equals(l);

    /// <inheritdoc/>
    public override int GetHashCode() => (Line, Column).GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => $"({Line},{Column})";

    /// <summary>
    /// Indicates whether one object is equal to another object of the same type.
    /// </summary>
    public static bool operator ==(Location left, Location right) => left.Equals(right);

    /// <summary>
    /// Indicates whether one object is not equal to another object of the same type.
    /// </summary>
    public static bool operator !=(Location left, Location right) => !(left == right);

    /// <summary>
    /// Deconstructs an instance of <see cref="Location"/> on line and column.
    /// </summary>
    public void Deconstruct(out int line, out int column)
    {
        line = Line;
        column = Column;
    }
}
