using System.Collections.Generic;
using System.Diagnostics;
using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser
{
    // WARNING: mutable struct, pass it by reference to those methods that will change it
    internal partial struct ParserContext
    {
        private delegate TResult ParseCallback<out TResult>(ref ParserContext context);

        private readonly ROM _source;
        private readonly IgnoreOptions _ignoreOptions;
        private GraphQLComment? _currentComment;
        private List<GraphQLComment>? _unattachedComments;
        private Token _currentToken;
        private Token _prevToken;
        private readonly GraphQLDocument _document;
        private int _currentDepth;
        private readonly int _maxDepth;

        public ParserContext(ROM source, ParserOptions options)
        {
            _currentComment = null;
            _unattachedComments = null;
            _source = source;
            _ignoreOptions = options.Ignore;
            _currentDepth = 1; // GraphQLDocument created here
            _maxDepth = options.MaxDepth ?? 128;
            // should create document beforehand to use RentedMemoryTracker while parsing comments
            _document = NodeHelper.CreateGraphQLDocument(options.Ignore);
            _currentToken = _prevToken = new Token
            (
                TokenKind.UNKNOWN,
                default,
                0,
                0
            );

            Advance();
        }

        private void IncreaseDepth()
        {
            // Encourage compiler inlining of this method by moving exception to a separate method
            if (++_currentDepth > _maxDepth)
                ThrowMaxDepthException();
        }

        private void DecreaseDepth() => --_currentDepth;

        private void ThrowMaxDepthException()
        {
            throw new GraphQLMaxDepthExceededException(_source, _currentToken.Start);
        }

        private readonly GraphQLLocation GetLocation(int start)
        {
            return new GraphQLLocation
            (
                start,
                _prevToken.End
            );
        }

        private List<T>? ZeroOrMore<T>(TokenKind open, ParseCallback<T> next, TokenKind close)
            where T : ASTNode
        {
            Expect(open);

            List<T>? nodes = null;
            while (!Skip(close))
                (nodes ??= new List<T>()).Add(next(ref this));

            return nodes;
        }

        private List<T> OneOrMore<T>(TokenKind open, ParseCallback<T> next, TokenKind close)
             where T : ASTNode
        {
            Expect(open);

            var nodes = new List<T> { next(ref this) };
            while (!Skip(close))
                nodes.Add(next(ref this));

            return nodes;
        }

        private readonly bool Peek(TokenKind kind) => _currentToken.Kind == kind;

        private bool Skip(TokenKind kind)
        {
            Debug.Assert(kind != TokenKind.COMMENT && kind != TokenKind.UNKNOWN);

            bool isCurrentTokenMatching = _currentToken.Kind == kind;

            if (isCurrentTokenMatching)
            {
                Advance();
            }

            return isCurrentTokenMatching;
        }

        private void Advance(bool fromParseComment = false)
        {
            // We should not advance further if we have already reached the EOF.
            if (_currentToken.Kind != TokenKind.EOF)
            {
                _prevToken = _currentToken;
                _currentToken = Lexer.Lex(_source, _currentToken.End);
                // Comments may appear everywhere
                if (!fromParseComment)
                    ParseComment();
            }
        }

        private void Expect(TokenKind kind)
        {
            if (_currentToken.Kind == kind)
            {
                Advance();
            }
            else
            {
                Throw_From_Expect(kind);
            }
        }

        private void Throw_From_Expect(TokenKind kind)
        {
            throw new GraphQLSyntaxErrorException($"Expected {Token.GetTokenKindDescription(kind)}, found {_currentToken}", _source, _currentToken.Start);
        }

        private void ExpectKeyword(string keyword)
        {
            if (_currentToken.Kind == TokenKind.NAME && _currentToken.Value == keyword)
                Advance();
            else
                Throw_From_ExpectKeyword(keyword);
        }

        private void Throw_From_ExpectKeyword(string keyword)
        {
            throw new GraphQLSyntaxErrorException($"Expected \"{keyword}\", found Name \"{_currentToken.Value}\"", _source, _currentToken.Start);
        }
    }
}
