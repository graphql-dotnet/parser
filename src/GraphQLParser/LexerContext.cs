namespace GraphQLParser
{
    using Exceptions;
    using System;

    public struct LexerContext
    {
        private int currentIndex;
        private readonly ISource source;
        private readonly ILexemeCache cache;

        public LexerContext(ISource source, int index) : this(source, index, NoCache.Instance)
        {
        }

        public LexerContext(ISource source, int index, ILexemeCache cache)
        {
            currentIndex = index;
            this.source = source;
            this.cache = cache ?? NoCache.Instance;
        }

        public Token GetToken()
        {
            if (source.Body == null)
                return CreateEOFToken();

            currentIndex = GetPositionAfterWhitespace(source.Body, currentIndex);

            if (currentIndex >= source.Body.Length)
                return CreateEOFToken();

            var code = source.Body[currentIndex];

            ValidateCharacterCode(code);

            var token = CheckForPunctuationTokens(code);
            if (token != null)
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
                $"Unexpected character {ResolveCharName(code, IfUnicodeGetString())}", source, currentIndex);
        }

        public bool OnlyHexInString(string test)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public Token ReadComment()
        {
            var start = currentIndex;

            var chunkStart = ++currentIndex;
            var code = GetCode();
            var value = string.Empty;

            while (IsNotAtTheEndOfQuery() && code != 0x000A && code != 0x000D)
            {
                code = ProcessCharacter(ref value, ref chunkStart);
            }

            value += source.Body.Substring(chunkStart, currentIndex - chunkStart);

            return new Token
            {
                Kind = TokenKind.COMMENT,
                Value = value,
                Start = start,
                End = currentIndex + 1
            };
        }

        public Token ReadNumber()
        {
            var isFloat = false;
            var start = currentIndex;
            var code = source.Body[start];

            if (code == '-')
                code = NextCode();

            var nextCode = code == '0'
                ? NextCode()
                : ReadDigitsFromOwnSource(code);

            if (nextCode >= 48 && nextCode <= 57)
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid number, unexpected digit after {code}: \"{nextCode}\"", source, currentIndex);
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
            var start = currentIndex;
            var value = ProcessStringChunks();

            return new Token
            {
                Kind = TokenKind.STRING,
                Value = value,
                Start = start,
                End = currentIndex + 1
            };
        }

        private static bool IsValidNameCharacter(char code)
        {
            return code == '_' || char.IsLetterOrDigit(code);
        }

        private string AppendCharactersFromLastChunk(string value, int chunkStart)
        {
            return value + source.Body.Substring(chunkStart, currentIndex - chunkStart - 1);
        }

        private string AppendToValueByCode(string value, char code)
        {
            value += code switch
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
                _ => throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\{code}.", source, currentIndex),
            };

            return value;
        }

        private int CharToHex(char code) => Convert.ToByte(code.ToString(), 16);

        private void CheckForInvalidCharacters(char code)
        {
            if (code < 0x0020 && code != 0x0009)
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid character within String: \\u{((int)code).ToString("D4")}.", source, currentIndex);
            }
        }

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
            _ => null
        };

        private Token CheckForSpreadOperator()
        {
            var char1 = source.Body.Length > currentIndex + 1 ? source.Body[currentIndex + 1] : 0;
            var char2 = source.Body.Length > currentIndex + 2 ? source.Body[currentIndex + 2] : 0;

            if (char1 == '.' && char2 == '.')
            {
                return CreatePunctuationToken(TokenKind.SPREAD, 3);
            }

            return null;
        }

        private void CheckStringTermination(char code)
        {
            if (code != '"')
            {
                throw new GraphQLSyntaxErrorException("Unterminated string.", source, currentIndex);
            }
        }

        private Token CreateEOFToken()
        {
            return new Token
            {
                Start = currentIndex,
                End = currentIndex,
                Kind = TokenKind.EOF
            };
        }

        private Token CreateFloatToken(int start)
        {
            return new Token
            {
                Kind = TokenKind.FLOAT,
                Start = start,
                End = currentIndex,
                Value = source.Body.Substring(start, currentIndex - start)
            };
        }

        private Token CreateIntToken(int start)
        {
            return new Token
            {
                Kind = TokenKind.INT,
                Start = start,
                End = currentIndex,
                Value = cache.GetInt(source.Body, start, currentIndex)
            };
        }

        private Token CreateNameToken(int start)
        {
            return new Token
            {
                Start = start,
                End = currentIndex,
                Kind = TokenKind.NAME,
                Value = cache.GetName(source.Body, start, currentIndex)
            };
        }

        private Token CreatePunctuationToken(TokenKind kind, int offset)
        {
            return new Token
            {
                Start = currentIndex,
                End = currentIndex + offset,
                Kind = kind,
                Value = null
            };
        }

        private char GetCode()
        {
            return IsNotAtTheEndOfQuery()
                ? source.Body[currentIndex]
                : (char)0;
        }

        private int GetPositionAfterWhitespace(string body, int start)
        {
            var position = start;

            while (position < body.Length)
            {
                var code = body[position];
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

        private char GetUnicodeChar()
        {
            if (currentIndex + 5 > source.Body.Length)
            {
                var truncatedExpression = source.Body.Substring(currentIndex);
                throw new GraphQLSyntaxErrorException($"Invalid character escape sequence at EOF: \\{truncatedExpression}.", source, currentIndex);
            }

            var expression = source.Body.Substring(currentIndex, 5);

            if (!OnlyHexInString(expression.Substring(1)))
            {
                throw new GraphQLSyntaxErrorException($"Invalid character escape sequence: \\{expression}.", source, currentIndex);
            }

            return (char)(
                CharToHex(NextCode()) << 12 |
                CharToHex(NextCode()) << 8 |
                CharToHex(NextCode()) << 4 |
                CharToHex(NextCode()));
        }

        private string IfUnicodeGetString()
        {
            return source.Body.Length > currentIndex + 5 &&
                OnlyHexInString(source.Body.Substring(currentIndex + 2, 4))
                ? source.Body.Substring(currentIndex, 6)
                : null;
        }

        private bool IsNotAtTheEndOfQuery() => currentIndex < source.Body.Length;

        private char NextCode()
        {
            currentIndex++;
            return IsNotAtTheEndOfQuery()
                ? source.Body[currentIndex]
                : (char)0;
        }

        private char ProcessCharacter(ref string value, ref int chunkStart)
        {
            var code = GetCode();
            ++currentIndex;

            if (code == '\\')
            {
                value = AppendToValueByCode(AppendCharactersFromLastChunk(value, chunkStart), GetCode());

                ++currentIndex;
                chunkStart = currentIndex;
            }

            return GetCode();
        }

        private string ProcessStringChunks()
        {
            var chunkStart = ++currentIndex;
            var code = GetCode();
            var value = string.Empty;

            while (IsNotAtTheEndOfQuery() && code != 0x000A && code != 0x000D && code != '"')
            {
                CheckForInvalidCharacters(code);
                code = ProcessCharacter(ref value, ref chunkStart);
            }

            CheckStringTermination(code);
            value += source.Body.Substring(chunkStart, currentIndex - chunkStart);
            return value;
        }

        private int ReadDigits(ISource source, int start, char firstCode)
        {
            var body = source.Body;
            var position = start;
            var code = firstCode;

            if (!char.IsNumber(code))
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid number, expected digit but got: {ResolveCharName(code)}", source, currentIndex);
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
            currentIndex = ReadDigits(source, currentIndex, code);
            return GetCode();
        }

        private Token ReadName()
        {
            var start = currentIndex;
            char code;

            do
            {
                currentIndex++;
                code = GetCode();
            }
            while (IsNotAtTheEndOfQuery() && IsValidNameCharacter(code));

            return CreateNameToken(start);
        }

        private string ResolveCharName(char code, string unicodeString = null)
        {
            if (code == '\0')
                return "<EOF>";

            if (!string.IsNullOrWhiteSpace(unicodeString))
                return $"\"{unicodeString}\"";

            return $"\"{code}\"";
        }

        private void ValidateCharacterCode(int code)
        {
            if (code < 0x0020 && code != 0x0009 && code != 0x000A && code != 0x000D)
            {
                throw new GraphQLSyntaxErrorException(
                    $"Invalid character \"\\u{code.ToString("D4")}\".", source, currentIndex);
            }
        }

        private int WaitForEndOfComment(string body, int position, char code)
        {
            while (++position < body.Length && (code = body[position]) != 0 && (code > 0x001F || code == 0x0009) && code != 0x000A && code != 0x000D)
            {
            }

            return position;
        }
    }
}