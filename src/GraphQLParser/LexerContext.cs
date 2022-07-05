using System.Diagnostics.CodeAnalysis;
using System.Text;
using GraphQLParser.Exceptions;

namespace GraphQLParser;

// WARNING: mutable ref struct, pass it by reference to those methods that will change it
internal ref struct LexerContext
{
    private int _currentIndex;
    private readonly ROM _source;

    public LexerContext(ROM source, int index)
    {
        _currentIndex = index;
        _source = source;
    }

    public Token GetToken()
    {
        if (_source.IsEmpty)
            return CreateEOFToken();

        _currentIndex = GetPositionAfterWhitespace();

        if (_currentIndex >= _source.Length)
            return CreateEOFToken();

        char code = _source.Span[_currentIndex];

        if (code < ' ' && code != '\t' && code != '\n' && code != '\r')
        {
            Throw_From_GetToken1(code);
        }

        var token = CheckForPunctuationTokens(code);
        if (token.Kind != TokenKind.UNKNOWN)
            return token;

        if (code == '#')
            return ReadComment();

        // http://spec.graphql.org/October2021/#NameStart
        if ('a' <= code && code <= 'z' || 'A' <= code && code <= 'Z' || code == '_')
            return ReadName();

        if ('0' <= code && code <= '9' || code == '-')
            return ReadNumber();

        if (code == '"')
        {
            return _currentIndex + 2 < _source.Length && _source.Span[_currentIndex + 1] == '"' && _source.Span[_currentIndex + 2] == '"'
                ? ReadBlockString()
                : ReadString();
        }

        return Throw_From_GetToken2(code);
    }

    [DoesNotReturn]
    private void Throw_From_GetToken1(int code)
    {
        throw new GraphQLSyntaxErrorException($"Invalid character \"\\u{code:D4}\".", _source, _currentIndex);
    }

    [DoesNotReturn]
    private Token Throw_From_GetToken2(char code)
    {
        throw new GraphQLSyntaxErrorException($"Unexpected character {ResolveCharName(code, IfUnicodeGetString())}", _source, _currentIndex);
    }

    private static bool OnlyHexInString(ReadOnlySpan<char> test)
    {
        for (int i = 0; i < test.Length; ++i)
        {
            char ch = test[i];
            if (!('0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F'))
                return false;
        }

        return true;
    }

    private Token ReadNumber()
    {
        bool isFloat = false;
        int start = _currentIndex;
        char code = _source.Span[start];

        if (code == '-')
            code = NextCode();

        char nextCode = code == '0'
            ? NextCode()
            : ReadDigitsFromOwnSource(code);

        if ('0' <= nextCode && nextCode <= '9')
        {
            Throw_From_ReadNumber(code, nextCode);
        }

        code = nextCode;
        if (code == '.')
        {
            isFloat = true;
            code = ReadDigitsFromOwnSource(NextCode());
        }

        if (code == 'E' || code == 'e')
        {
            isFloat = true;
            code = NextCode();
            if (code == '+' || code == '-')
            {
                code = NextCode();
            }

            _ = ReadDigitsFromOwnSource(code);
        }

        return isFloat ? CreateFloatToken(start) : CreateIntToken(start);
    }

    [DoesNotReturn]
    private void Throw_From_ReadNumber(char code, char nextCode)
    {
        throw new GraphQLSyntaxErrorException($"Invalid number, unexpected digit after {code}: \"{nextCode}\"", _source, _currentIndex);
    }

    private Token ReadComment()
    {
        int start = _currentIndex;
        char code = NextCode();

        // The buffer on the stack allows to get rid of intermediate heap allocations if the string
        // 1) not too long
        // or
        // 2) does not contain escape sequences.
        Span<char> buffer = stackalloc char[Math.Min(_source.Length - _currentIndex + 32, 4096)];
        StringBuilder? sb = null;

        int index = 0;
        bool escaped = false;

        while (_currentIndex < _source.Length && code != 0x000A && code != 0x000D)
        {
            char ch = ReadCharacterFromString(code, ref escaped);

            try
            {
                buffer[index++] = ch;
            }
            catch (IndexOutOfRangeException) // fallback to StringBuilder in case of buffer overflow
            {
                sb ??= new StringBuilder(buffer.Length * 2);

                for (int i = 0; i < buffer.Length; ++i)
                    sb.Append(buffer[i]);

                sb.Append(ch);
                index = 0;
            }

            code = NextCode();
        }

        if (sb != null)
        {
            for (int i = 0; i < index; ++i)
                sb.Append(buffer[i]);
        }

        var value = escaped
            ? (sb == null ? buffer.Slice(0, index).ToString() : sb.ToString()) // allocate string from either buffer on stack or heap
            : _source.Slice(start + 1, _currentIndex - start - 1); // the best case, no escaping so no need to allocate

        return new Token
        (
            TokenKind.COMMENT,
            value,
            start,
            _currentIndex + 1
        );
    }

    // TODO: this method can still be optimized no not allocate at all if block string:
    //
    // 1) not too long
    // 2) has no escape sequences
    // 3) has no '\r' characters
    // 4) has no initial whitespace on each line, ignoring the first line (or, has no '\n' characters)
    //
    // In this case, ROM for the returned token represents unmodified part of the source ROM,
    // so it can be just sliced from '_source' as you can see in more simple ReadString method.
    private Token ReadBlockString()
    {
        int start = _currentIndex += 2; // skip ""
        char code = NextCode();

        Span<char> buffer = stackalloc char[Math.Min(_source.Length - _currentIndex + 32, 4096)];
        StringBuilder? sb = null;

        int index = 0;
        bool escape = false; // when the last character was \
        bool lastWasCr = false;

        while (_currentIndex < _source.Length)
        {
            if (code < 0x0020 && code != 0x0009 && code != 0x000A && code != 0x000D)
            {
                Throw_From_ReadBlockString1(code);
            }

            // check for """
            if (code == '"' && _currentIndex + 2 < _source.Length && _source.Span[_currentIndex + 1] == '"' && _source.Span[_currentIndex + 2] == '"')
            {
                // if last character was \ then go ahead and write out the """, skipping the \
                if (escape)
                {
                    escape = false;
                }
                else
                {
                    // end of block string
                    break;
                }
            }
            else if (escape)
            {
                // last character was \ so write the \ and then retry this character with escaped = false
                code = '\\';
                _currentIndex--;
                escape = false;
            }
            else if (code == '\\')
            {
                // this character is a \ so don't write anything yet, but check the next character
                escape = true;
                code = NextCode();
                lastWasCr = false;
                continue;
            }
            else
            {
                escape = false;
            }


            if (!(lastWasCr && code == '\n'))
            {
                // write code
                if (index < buffer.Length)
                {
                    buffer[index++] = code == '\r' ? '\n' : code;
                }
                else // fallback to StringBuilder in case of buffer overflow
                {
                    sb ??= new StringBuilder(buffer.Length * 2);

                    for (int i = 0; i < buffer.Length; ++i)
                        sb.Append(buffer[i]);

                    sb.Append(code == '\r' ? '\n' : code);
                    index = 0;
                }
            }

            lastWasCr = code == '\r';

            code = NextCode();
        }

        if (_currentIndex >= _source.Length)
        {
            Throw_From_ReadBlockString2();
        }
        _currentIndex += 2; // skip ""

        if (sb != null)
        {
            for (int i = 0; i < index; ++i)
                sb.Append(buffer[i]);
        }

        // at this point, if sb != null, then sb has the whole string, otherwise buffer (of length index) has the whole string
        // also, all line termination combinations have been replaced with LF

        ROM value;
        if (sb != null)
        {
            var chars = new char[sb.Length];
            sb.CopyTo(0, chars, 0, sb.Length);
            value = ProcessBuffer(chars);
        }
        else
        {
            value = ProcessBuffer(buffer.Slice(0, index));
        }

        return new Token
        (
            TokenKind.STRING,
            value,
            start,
            _currentIndex + 1
        );

        static ROM ProcessBuffer(Span<char> buffer)
        {
            // scan string to determine maximum valid commonIndent value,
            // number of initial blank lines, and number of trailing blank lines
            int commonIndent = int.MaxValue;
            int initialBlankLines = 1;
            int skipLinesAfter; // skip all text after line ###, as determined by the number of trailing blank lines
            {
                int trailingBlankLines = 0;
                int line = 0;
                int whitespace = 0;
                bool allWhitespace = true;
                bool reachedCharacter = false;
                for (int index = 0; index < buffer.Length; index++)
                {
                    char code = buffer[index];
                    if (code == '\n')
                    {
                        if (allWhitespace)
                            trailingBlankLines += 1;
                        if (line != 0 && !allWhitespace && whitespace < commonIndent)
                            commonIndent = whitespace;
                        line++;
                        whitespace = 0;
                        allWhitespace = true;
                        if (!reachedCharacter)
                            initialBlankLines++;
                    }
                    else if (code == ' ' || code == '\t')
                    {
                        if (allWhitespace)
                            whitespace++;
                    }
                    else
                    {
                        allWhitespace = false;
                        if (!reachedCharacter)
                            initialBlankLines--;
                        reachedCharacter = true;
                        trailingBlankLines = 0;
                    }
                }
                if (allWhitespace)
                    trailingBlankLines += 1;
                if (line != 0 && !allWhitespace && whitespace < commonIndent)
                    commonIndent = whitespace;
                if (commonIndent == int.MaxValue)
                    commonIndent = 0;
                int lines = line + 1;
                skipLinesAfter = lines - trailingBlankLines;
            }

            // step through the input, skipping the initial blank lines and the trailing blank lines,
            // and skipping the initial blank characters from the start of each line
            Span<char> output = buffer.Length <= 4096 ? stackalloc char[buffer.Length] : new char[buffer.Length];
            int outputIndex = 0;
            {
                int line = 0;
                int col = 0;
                for (int index = 0; index < buffer.Length; index++)
                {
                    char code = buffer[index];
                    if (code == '\n')
                    {
                        if (++line >= skipLinesAfter)
                            break;
                        col = 0;
                        if (line > initialBlankLines)
                            output[outputIndex++] = code;
                    }
                    else
                    {
                        if (line >= initialBlankLines && (line == 0 || col++ >= commonIndent))
                            output[outputIndex++] = code;
                    }
                }
            }

            // return the string value from the output buffer
            return output.Slice(0, outputIndex).ToString(); //TODO: allocation
        }
    }

    private Token ReadString()
    {
        int start = _currentIndex;
        char code = NextCode();

        Span<char> buffer = stackalloc char[Math.Min(_source.Length - _currentIndex + 32, 4096)];
        StringBuilder? sb = null;

        int index = 0;
        bool escaped = false;

        while (_currentIndex < _source.Length && code != 0x000A && code != 0x000D && code != '"')
        {
            if (code < 0x0020 && code != 0x0009)
            {
                Throw_From_ReadString1(code);
            }

            char ch = ReadCharacterFromString(code, ref escaped);

            try
            {
                buffer[index++] = ch;
            }
            catch (IndexOutOfRangeException) // fallback to StringBuilder in case of buffer overflow
            {
                sb ??= new StringBuilder(buffer.Length * 2);

                for (int i = 0; i < buffer.Length; ++i)
                    sb.Append(buffer[i]);

                sb.Append(ch);
                index = 0;
            }

            code = NextCode();
        }

        if (code != '"')
        {
            Throw_From_ReadString2();
        }

        if (sb != null)
        {
            for (int i = 0; i < index; ++i)
                sb.Append(buffer[i]);
        }

        var value = escaped
            ? (sb == null ? buffer.Slice(0, index).ToString() : sb.ToString()) // allocate string from either buffer on stack or heap
            : _source.Slice(start + 1, _currentIndex - start - 1); // the best case, no escaping so no need to allocate

        return new Token
        (
            TokenKind.STRING,
            value,
            start,
            _currentIndex + 1
        );
    }

    [DoesNotReturn]
    private void Throw_From_ReadString1(char code)
    {
        throw new GraphQLSyntaxErrorException($"Invalid character within String: \\u{(int)code:D4}.", _source, _currentIndex);
    }

    [DoesNotReturn]
    private void Throw_From_ReadString2()
    {
        throw new GraphQLSyntaxErrorException("Unterminated string.", _source, _currentIndex);
    }

    [DoesNotReturn]
    private void Throw_From_ReadBlockString1(char code)
    {
        throw new GraphQLSyntaxErrorException($"Invalid character within block string: \\u{(int)code:D4}.", _source, _currentIndex);
    }

    [DoesNotReturn]
    private void Throw_From_ReadBlockString2()
    {
        throw new GraphQLSyntaxErrorException("Unterminated block string.", _source, _currentIndex);
    }

    // sets escaped only to true
    private char ReadCharacterFromString(char currentCharacter, ref bool escaped)
    {
        if (currentCharacter == '\\')
        {
            escaped = true;
            char escapedChar = NextCode();

            return escapedChar switch
            {
                '"' => '"',
                '/' => '/',
                '\\' => '\\',
                'b' => '\b',
                'f' => '\f',
                'n' => '\n',
                'r' => '\r',
                't' => '\t',
                'u' => GetUnicodeChar(),
                _ => Throw_From_ReadCharacterFromString(escapedChar)
            };
        }
        else
        {
            return currentCharacter;
        }
    }

    [DoesNotReturn]
    private char Throw_From_ReadCharacterFromString(char escapedChar)
    {
        throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\{escapedChar}.", _source, _currentIndex);
    }

    private char GetUnicodeChar()
    {
        if (_currentIndex + 5 > _source.Length)
        {
            string truncatedExpression = _source.Span.Slice(_currentIndex).ToString();
            Throw_From_GetUnicodeChar1(truncatedExpression);
        }

        var expression = _source.Span.Slice(_currentIndex + 1, 4);

        if (!OnlyHexInString(expression))
        {
            Throw_From_GetUnicodeChar2(expression);
        }

        return (char)(
            CharToHex(NextCode()) << 12 |
            CharToHex(NextCode()) << 8 |
            CharToHex(NextCode()) << 4 |
            CharToHex(NextCode()));
    }

    [DoesNotReturn]
    private void Throw_From_GetUnicodeChar1(string truncatedExpression)
    {
        throw new GraphQLSyntaxErrorException($"Invalid character escape sequence at EOF: \\{truncatedExpression}.", _source, _currentIndex);
    }

    [DoesNotReturn]
    private void Throw_From_GetUnicodeChar2(ReadOnlySpan<char> expression)
    {
        throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\u{expression.ToString()}.", _source, _currentIndex);
    }

    private static int CharToHex(char code) => Convert.ToByte(code.ToString(), 16);

    private Token CheckForPunctuationTokens(char code) => code switch
    {
        '!' => CreatePunctuationToken(TokenKind.BANG, 1),
        '$' => CreatePunctuationToken(TokenKind.DOLLAR, 1),
        '(' => CreatePunctuationToken(TokenKind.PAREN_L, 1),
        ')' => CreatePunctuationToken(TokenKind.PAREN_R, 1),
        '.' => CheckForSpreadOperator(),
        ':' => CreatePunctuationToken(TokenKind.COLON, 1),
        '=' => CreatePunctuationToken(TokenKind.EQUALS, 1),
        '@' => CreatePunctuationToken(TokenKind.AT, 1),
        '&' => CreatePunctuationToken(TokenKind.AMPERSAND, 1),
        '[' => CreatePunctuationToken(TokenKind.BRACKET_L, 1),
        ']' => CreatePunctuationToken(TokenKind.BRACKET_R, 1),
        '{' => CreatePunctuationToken(TokenKind.BRACE_L, 1),
        '}' => CreatePunctuationToken(TokenKind.BRACE_R, 1),
        '|' => CreatePunctuationToken(TokenKind.PIPE, 1),
        _ => CreateUnknownToken()
    };

    private Token CheckForSpreadOperator()
    {
        int char1 = _source.Length > _currentIndex + 1 ? _source.Span[_currentIndex + 1] : 0;
        int char2 = _source.Length > _currentIndex + 2 ? _source.Span[_currentIndex + 2] : 0;

        return char1 == '.' && char2 == '.'
            ? CreatePunctuationToken(TokenKind.SPREAD, 3)
            : CreateUnknownToken();
    }

    private Token CreateUnknownToken()
    {
        return new Token
        (
            TokenKind.UNKNOWN,
            default,
            _currentIndex,
            _currentIndex
        );
    }

    private Token CreateEOFToken()
    {
        return new Token
        (
            TokenKind.EOF,
            default,
            _currentIndex,
            _currentIndex
        );
    }

    private Token CreateFloatToken(int start)
    {
        return new Token
        (
            TokenKind.FLOAT,
            _source.Slice(start, _currentIndex - start),
            start,
            _currentIndex
        );
    }

    private Token CreateIntToken(int start)
    {
        return new Token
        (
            TokenKind.INT,
            _source.Slice(start, _currentIndex - start),
            start,
            _currentIndex
        );
    }

    private Token CreateNameToken(int start)
    {
        return new Token
        (
            TokenKind.NAME,
            _source.Slice(start, _currentIndex - start),
            start,
            _currentIndex
        );
    }

    private Token CreatePunctuationToken(TokenKind kind, int offset)
    {
        return new Token
        (
            kind,
            _source.Slice(_currentIndex, offset),
            _currentIndex,
            _currentIndex + offset
        );
    }

    private int GetPositionAfterWhitespace()
    {
        int position = _currentIndex;

        while (position < _source.Length)
        {
            switch (_source.Span[position])
            {
                case '\xFEFF': // BOM
                case '\t': // tab
                case ' ': // space
                case '\n': // new line
                case '\r': // carriage return
                case ',': // Comma
                    ++position;
                    break;

                default:
                    return position;
            }
        }

        return position;
    }

    private string? IfUnicodeGetString()
    {
        return _source.Length > _currentIndex + 5 && OnlyHexInString(_source.Span.Slice(_currentIndex + 2, 4))
            ? _source.Span.Slice(_currentIndex, 6).ToString()
            : null;
    }

    private char NextCode()
    {
        ++_currentIndex;
        return _currentIndex < _source.Length
            ? _source.Span[_currentIndex]
            : (char)0;
    }

    private int ReadDigits(int start, char firstCode)
    {
        int position = start;
        char code = firstCode;

        if (code < '0' || '9' < code)
        {
            Throw_From_ReadDigits(code);
        }

        var body = _source.Span;

        do
        {
            code = ++position < body.Length
                ? body[position]
                : (char)0;
        }
        while ('0' <= code && code <= '9');

        return position;
    }

    [DoesNotReturn]
    private void Throw_From_ReadDigits(char code)
    {
        throw new GraphQLSyntaxErrorException($"Invalid number, expected digit but got: {ResolveCharName(code)}", _source, _currentIndex);
    }

    private char ReadDigitsFromOwnSource(char code)
    {
        _currentIndex = ReadDigits(_currentIndex, code);
        return _currentIndex < _source.Length
            ? _source.Span[_currentIndex]
            : (char)0;
    }

    // http://spec.graphql.org/October2021/#Name
    private Token ReadName()
    {
        int start = _currentIndex;
        char code;

        do
        {
            if (++_currentIndex < _source.Length)
                code = _source.Span[_currentIndex];
            else
                break;
        }
        while (code == '_' || 'a' <= code && code <= 'z' || 'A' <= code && code <= 'Z' || '0' <= code && code <= '9');

        return CreateNameToken(start);
    }

    private static string ResolveCharName(char code, string? unicodeString = null)
    {
        if (code == '\0')
            return "<EOF>";

        return string.IsNullOrWhiteSpace(unicodeString)
            ? $"\"{code}\""
            : $"\"{unicodeString}\"";
    }
}
