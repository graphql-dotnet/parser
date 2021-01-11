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

        public LexerContext(ReadOnlyMemory<char> source, int index)
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

            if ('a' <= code && code <= 'z' || 'A' <= code && code <= 'Z' || code == '_')
                return ReadName();

            if ('0' <= code && code <= '9' || code == '-')
                return ReadNumber();

            if (code == '"')
                return ReadString();

            return Throw_From_GetToken2(code);
        }

        private void Throw_From_GetToken1(int code)
        {
            throw new GraphQLSyntaxErrorException($"Invalid character \"\\u{code:D4}\".", _source.Span, _currentIndex);
        }

        private Token Throw_From_GetToken2(char code)
        {
            throw new GraphQLSyntaxErrorException($"Unexpected character {ResolveCharName(code, IfUnicodeGetString())}", _source.Span, _currentIndex);
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

                code = ReadDigitsFromOwnSource(code);
            }

            return isFloat ? CreateFloatToken(start) : CreateIntToken(start);
        }

        private void Throw_From_ReadNumber(char code, char nextCode)
        {
            throw new GraphQLSyntaxErrorException($"Invalid number, unexpected digit after {code}: \"{nextCode}\"", _source.Span, _currentIndex);
        }

        private Token ReadComment()
        {
            int start = _currentIndex;
            char code = NextCode();

            Span<char> buffer = stackalloc char[4096];
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

        private Token ReadString()
        {
            int start = _currentIndex;
            char code = NextCode();

            Span<char> buffer = stackalloc char[4096];
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
                Throw_From_ReadString2();
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

        private void Throw_From_ReadString1(char code)
        {
            throw new GraphQLSyntaxErrorException($"Invalid character within String: \\u{(int)code:D4}.", _source.Span, _currentIndex);
        }

        private void Throw_From_ReadString2()
        {
            throw new GraphQLSyntaxErrorException("Unterminated string.", _source.Span, _currentIndex);
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

        private char Throw_From_ReadCharacterFromString(char escapedChar)
        {
            throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\{escapedChar}.", _source.Span, _currentIndex);
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

        private void Throw_From_GetUnicodeChar1(string truncatedExpression)
        {
            throw new GraphQLSyntaxErrorException($"Invalid character escape sequence at EOF: \\{truncatedExpression}.", _source.Span, _currentIndex);
        }

        private void Throw_From_GetUnicodeChar2(ReadOnlySpan<char> expression)
        {
            throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\u{expression.ToString()}.", _source.Span, _currentIndex);
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
            var body = _source.Span;

            while (position < _source.Length)
            {
                char code = body[position];
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

            var body = _source.Span;

            if (code < '0' || '9' < code)
            {
                Throw_From_ReadDigits(code, body);
            }

            do
            {
                code = ++position < body.Length
                    ? body[position]
                    : (char)0;
            }
            while ('0' <= code && code <= '9');

            return position;
        }

        private void Throw_From_ReadDigits(char code, ReadOnlySpan<char> body)
        {
            throw new GraphQLSyntaxErrorException($"Invalid number, expected digit but got: {ResolveCharName(code)}", body, _currentIndex);
        }

        private char ReadDigitsFromOwnSource(char code)
        {
            _currentIndex = ReadDigits(_currentIndex, code);
            return _currentIndex < _source.Length
                ? _source.Span[_currentIndex]
                : (char)0;
        }

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

        private string ResolveCharName(char code, string? unicodeString = null)
        {
            if (code == '\0')
                return "<EOF>";

            return string.IsNullOrWhiteSpace(unicodeString)
                ? $"\"{code}\""
                : $"\"{unicodeString}\"";
        }
    }
}
