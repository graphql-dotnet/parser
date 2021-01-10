using GraphQLParser.Exceptions;
using System;
using System.Text;

namespace GraphQLParser
{
    // WARNING: mutable struct, pass it by reference to those methods that will change it
    internal struct LexerContext
    {
        private int _currentIndex;
        private readonly ReadOnlyMemory<char> _source;
        private readonly int _bufferSize;

        public LexerContext(ReadOnlyMemory<char> source, int index, int? bufferSize)
        {
            _currentIndex = index;
            _source = source;
            _bufferSize = bufferSize == null ? 4096 : bufferSize.Value;
        }

        public Token GetToken()
        {
            if (_source.IsEmpty)
                return CreateEOFToken();

            _currentIndex = GetPositionAfterWhitespace(_source, _currentIndex);

            if (_currentIndex >= _source.Length)
                return CreateEOFToken();

            char code = _source.Span[_currentIndex];

            ValidateCharacterCode(code);

            var token = CheckForPunctuationTokens(code);
            if (token.Kind != TokenKind.UNKNOWN)
                return token;

            if (code == '#')
                return ReadComment();

            if (char.IsLetter(code) || code == '_')
                return ReadName();

            if (char.IsNumber(code) || code == '-')
                return ReadNumber();

            if (code == '"')
                return ReadString();

            throw new GraphQLSyntaxErrorException(
                $"Unexpected character {ResolveCharName(code, IfUnicodeGetString())}", _source, _currentIndex);
        }

        public static bool OnlyHexInString(ReadOnlySpan<char> test)
        {
            for (int i=0; i<test.Length; ++i)
            {
                char ch = test[i];
                if (!('0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F'))
                    return false;
            }

            return true;
        }

        public Token ReadNumber()
        {
            bool isFloat = false;
            int start = _currentIndex;
            char code = _source.Span[start];

            if (code == '-')
                code = NextCode();

            char nextCode = code == '0'
                ? NextCode()
                : ReadDigitsFromOwnSource(code);

            if (nextCode >= 48 && nextCode <= 57)
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid number, unexpected digit after {code}: \"{nextCode}\"", _source, _currentIndex);
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

                code = ReadDigitsFromOwnSource(code);
            }

            return isFloat ? CreateFloatToken(start) : CreateIntToken(start);
        }

        public Token ReadComment()
        {
            int start = _currentIndex;
            char code = NextCode();

            Span<char> buffer = stackalloc char[_bufferSize];
            StringBuilder? sb = null;

            int index = 0;
            bool escaped = false;

            while (IsNotAtTheEndOfQuery() && code != 0x000A && code != 0x000D)
            {
                char ch = ReadCharacterFromString(code, ref escaped);

                try
                {
                    buffer[index++] = ch;
                }
                catch (IndexOutOfRangeException) // fallback to StringBuilder in case of buffer overflow
                {
                    if (sb == null)
                        sb = new StringBuilder(buffer.Length * 2);

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
                ? (sb == null ? buffer.Slice(0, index).ToString() : sb.ToString()).AsMemory() // allocate string from either buffer on stack or heap
                : _source.Slice(start + 1, _currentIndex - start - 1); // the best case, no escaping so no need to allocate

            return new Token
            (
                TokenKind.COMMENT,
                value,
                start,
                _currentIndex + 1
            );
        }

        public Token ReadString()
        {
            int start = _currentIndex;
            char code = NextCode();

            Span<char> buffer = stackalloc char[_bufferSize];
            StringBuilder? sb = null;

            int index = 0;
            bool escaped = false;

            while (IsNotAtTheEndOfQuery() && code != 0x000A && code != 0x000D && code != '"')
            {
                if (code < 0x0020 && code != 0x0009)
                {
                    throw new GraphQLSyntaxErrorException($"Invalid character within String: \\u{(int)code:D4}.", _source, _currentIndex);
                }

                char ch = ReadCharacterFromString(code, ref escaped);

                try
                {
                    buffer[index++] = ch;
                }
                catch (IndexOutOfRangeException) // fallback to StringBuilder in case of buffer overflow
                {
                    if (sb == null)
                        sb = new StringBuilder(buffer.Length * 2);

                    for (int i = 0; i < buffer.Length; ++i)
                        sb.Append(buffer[i]);

                    sb.Append(ch);
                    index = 0;
                }

                code = NextCode();
            }

            if (code != '"')
            {
                throw new GraphQLSyntaxErrorException("Unterminated string.", _source, _currentIndex);
            }

            if (sb != null)
            {
                for (int i = 0; i < index; ++i)
                    sb.Append(buffer[i]);
            }

            var value = escaped
                ? (sb == null ? buffer.Slice(0, index).ToString() : sb.ToString()).AsMemory() // allocate string from either buffer on stack or heap
                : _source.Slice(start + 1, _currentIndex - start - 1); // the best case, no escaping so no need to allocate

            return new Token
            (
                TokenKind.STRING,
                value,
                start,
                _currentIndex + 1
            );
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
                    _ => throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\{escapedChar}.", _source, _currentIndex),
                };
            }
            else
            {
                return currentCharacter;
            }
        }

        private char GetUnicodeChar()
        {
            if (_currentIndex + 5 > _source.Length)
            {
                string truncatedExpression = _source.Span.Slice(_currentIndex).ToString();
                throw new GraphQLSyntaxErrorException($"Invalid character escape sequence at EOF: \\{truncatedExpression}.", _source, _currentIndex);
            }

            var expression = _source.Span.Slice(_currentIndex + 1, 4);

            if (!OnlyHexInString(expression))
            {
                throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\u{expression.ToString()}.", _source, _currentIndex);
            }

            return (char)(
                CharToHex(NextCode()) << 12 |
                CharToHex(NextCode()) << 8 |
                CharToHex(NextCode()) << 4 |
                CharToHex(NextCode()));
        }

        private static bool IsValidNameCharacter(char code) => code == '_' || char.IsLetterOrDigit(code);

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
                null,
                _currentIndex,
                _currentIndex
            );
        }

        private Token CreateEOFToken()
        {
            return new Token
            (
                TokenKind.EOF,
                null,
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

        private static int GetPositionAfterWhitespace(ReadOnlyMemory<char> body, int start)
        {
            int position = start;

            var span = body.Span;
            while (position < body.Length)
            {
                char code = span[position];
                switch (code)
                {
                    case '\xFEFF': // BOM
                    case '\t': // tab
                    case ' ': // space
                    case '\n': // new line
                    case '\r': // carriage return
                    case ',': // Comma
                        ++position;
                        break;

//                    case '#':
//                        position = WaitForEndOfComment(body, position, code);
//                        break;

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

        private bool IsNotAtTheEndOfQuery() => _currentIndex < _source.Length;

        private char GetCode()
        {
            return IsNotAtTheEndOfQuery()
                ? _source.Span[_currentIndex]
                : (char)0;
        }

        private char NextCode()
        {
            _currentIndex++;
            return IsNotAtTheEndOfQuery()
                ? _source.Span[_currentIndex]
                : (char)0;
        }

        private int ReadDigits(ReadOnlyMemory<char> source, int start, char firstCode)
        {
            int position = start;
            char code = firstCode;

            if (!char.IsNumber(code))
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid number, expected digit but got: {ResolveCharName(code)}", source, _currentIndex);
            }

            var s = source.Span;

            do
            {
                code = ++position < source.Length
                    ? s[position]
                    : (char)0;
            }
            while (char.IsNumber(code));

            return position;
        }

        private char ReadDigitsFromOwnSource(char code)
        {
            _currentIndex = ReadDigits(_source, _currentIndex, code);
            return GetCode();
        }

        private Token ReadName()
        {
            int start = _currentIndex;
            char code;

            do
            {
                _currentIndex++;
                code = GetCode();
            }
            while (IsNotAtTheEndOfQuery() && IsValidNameCharacter(code));

            return CreateNameToken(start);
        }

        private string ResolveCharName(char code, string? unicodeString = null)
        {
            if (code == '\0')
                return "<EOF>";

            return string.IsNullOrWhiteSpace(unicodeString)
                ? $"\"{code}\""
                : $"\"{unicodeString}\"";
        }

        private void ValidateCharacterCode(int code)
        {
            if (code < 0x0020 && code != 0x0009 && code != 0x000A && code != 0x000D)
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid character \"\\u{code:D4}\".", _source, _currentIndex);
            }
        }

        //private int WaitForEndOfComment(string body, int position, char code)
        //{
        //    while (++position < body.Length && (code = body[position]) != 0 && (code > 0x001F || code == 0x0009) && code != 0x000A && code != 0x000D)
        //    {
        //    }

        //    return position;
        //}
    }
}
