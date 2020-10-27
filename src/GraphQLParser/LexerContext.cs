using GraphQLParser.Exceptions;
using System;
using System.Text;

namespace GraphQLParser
{
    // WARNING: mutable struct, pass it by reference to those methods that will change it
    internal struct LexerContext
    {
        private int _currentIndex;
        private readonly ISource _source;
        private readonly ILexemeCache _cache;

        public LexerContext(ISource source, int index, ILexemeCache? cache)
        {
            _currentIndex = index;
            _source = source;
            _cache = cache ?? NoCache.Instance;
        }

        public Token GetToken()
        {
            if (_source.Body == null)
                return CreateEOFToken();

            _currentIndex = GetPositionAfterWhitespace(_source.Body, _currentIndex);

            if (_currentIndex >= _source.Body.Length)
                return CreateEOFToken();

            char code = _source.Body[_currentIndex];

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

        public bool OnlyHexInString(string test) => System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");

        public Token ReadComment()
        {
            int start = _currentIndex;
            char code = NextCode();
            var sb = new StringBuilder(); // TODO: buffer allocation; it can be pooled later

            while (IsNotAtTheEndOfQuery() && code != 0x000A && code != 0x000D)
            {
                ReadCharacterFromString(sb, code);
                code = NextCode();
            }

            return new Token
            (
                TokenKind.COMMENT,
                sb.ToString(),
                start,
                _currentIndex + 1
            );
        }

        public Token ReadNumber()
        {
            bool isFloat = false;
            int start = _currentIndex;
            char code = _source.Body[start];

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

        public Token ReadString()
        {
            int start = _currentIndex;
            char code = NextCode();
            var sb = new StringBuilder(); // TODO: buffer allocation; it can be pooled later

            while (IsNotAtTheEndOfQuery() && code != 0x000A && code != 0x000D && code != '"')
            {
                if (code < 0x0020 && code != 0x0009)
                {
                    throw new GraphQLSyntaxErrorException($"Invalid character within String: \\u{((int)code).ToString("D4")}.", _source, _currentIndex);
                }
                ReadCharacterFromString(sb, code);
                code = NextCode();
            }

            if (code != '"')
            {
                throw new GraphQLSyntaxErrorException("Unterminated string.", _source, _currentIndex);
            }

            return new Token
            (
                TokenKind.STRING,
                sb.ToString(),
                start,
                _currentIndex + 1
            );
        }

        private void ReadCharacterFromString(StringBuilder sb, char currentCharacter)
        {
            if (currentCharacter == '\\')
            {
                char escapedChar = NextCode();

                switch (escapedChar)
                {
                    case '"':
                        sb.Append('"');
                        break;
                    case '/':
                        sb.Append('/');
                        break;
                    case '\\':
                        sb.Append('\\');
                        break;
                    case 'b':
                        sb.Append('\b');
                        break;
                    case 'f':
                        sb.Append('\f');
                        break;
                    case 'n':
                        sb.Append('\n');
                        break;
                    case 'r':
                        sb.Append('\r');
                        break;
                    case 't':
                        sb.Append('\t');
                        break;
                    case 'u':
                        sb.Append(GetUnicodeChar());
                        break;
                    default:
                        throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\{escapedChar}.", _source, _currentIndex);
                };
            }
            else
            {
                sb.Append(currentCharacter);
            }
        }

        private char GetUnicodeChar()
        {
            if (_currentIndex + 5 > _source.Body.Length)
            {
                string truncatedExpression = _source.Body.Substring(_currentIndex);
                throw new GraphQLSyntaxErrorException($"Invalid character escape sequence at EOF: \\{truncatedExpression}.", _source, _currentIndex);
            }

            string expression = _source.Body.Substring(_currentIndex + 1, 4); // TODO: allocation

            if (!OnlyHexInString(expression))
            {
                throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\u{expression}.", _source, _currentIndex);
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
            '|' => CreatePunctuationToken(TokenKind.PIPE, 1),
            '}' => CreatePunctuationToken(TokenKind.BRACE_R, 1),
            _ => CreateUnknownToken()
        };

        private Token CheckForSpreadOperator()
        {
            int char1 = _source.Body.Length > _currentIndex + 1 ? _source.Body[_currentIndex + 1] : 0;
            int char2 = _source.Body.Length > _currentIndex + 2 ? _source.Body[_currentIndex + 2] : 0;

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
                _source.Body.Substring(start, _currentIndex - start),
                start,
                _currentIndex
            );
        }

        private Token CreateIntToken(int start)
        {
            return new Token
            (
                TokenKind.INT,
                _cache.GetInt(_source.Body, start, _currentIndex),
                start,
                _currentIndex
            );
        }

        private Token CreateNameToken(int start)
        {
            return new Token
            (
                TokenKind.NAME,
                _cache.GetName(_source.Body, start, _currentIndex),
                start,
                _currentIndex
            );
        }

        private Token CreatePunctuationToken(TokenKind kind, int offset)
        {
            return new Token
            (
                kind,
                null,
                _currentIndex,
                _currentIndex + offset
            );
        }

        private int GetPositionAfterWhitespace(string body, int start)
        {
            int position = start;

            while (position < body.Length)
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
            return _source.Body.Length > _currentIndex + 5 &&
                OnlyHexInString(_source.Body.Substring(_currentIndex + 2, 4))
                ? _source.Body.Substring(_currentIndex, 6)
                : null;
        }

        private bool IsNotAtTheEndOfQuery() => _currentIndex < _source.Body.Length;

        private char GetCode()
        {
            return IsNotAtTheEndOfQuery()
                ? _source.Body[_currentIndex]
                : (char)0;
        }

        private char NextCode()
        {
            _currentIndex++;
            return IsNotAtTheEndOfQuery()
                ? _source.Body[_currentIndex]
                : (char)0;
        }

        private int ReadDigits(ISource source, int start, char firstCode)
        {
            string body = source.Body;
            int position = start;
            char code = firstCode;

            if (!char.IsNumber(code))
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid number, expected digit but got: {ResolveCharName(code)}", source, _currentIndex);
            }

            do
            {
                code = ++position < body.Length
                    ? body[position]
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
                    $"Invalid character \"\\u{code.ToString("D4")}\".", _source, _currentIndex);
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
