using System.Collections.Generic;
using System.Diagnostics;
using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser;

// WARNING: mutable struct, pass it by reference to those methods that will change it
internal partial struct ParserContext
{
    private static string[] TopLevelKeywordOneOf { get; set; } = new[]
    {
            "query",
            "mutation",
            "subscription",
            "fragment",
            "schema",
            "scalar",
            "type",
            "interface",
            "union",
            "enum",
            "input",
            "extend",
            "directive",
        };

    private static string[] OperationTypeOneOf { get; set; } = new[]
    {
            "query",
            "mutation",
            "subscription",
        };

    private static string[] DirectiveLocationOneOf { get; set; } = new[]
    {
            // http://spec.graphql.org/June2018/#ExecutableDirectiveLocation
            "QUERY",
            "MUTATION",
            "SUBSCRIPTION",
            "FIELD",
            "FRAGMENT_DEFINITION",
            "FRAGMENT_SPREAD",
            "INLINE_FRAGMENT",
            "VARIABLE_DEFINITION",
            // http://spec.graphql.org/June2018/#TypeSystemDirectiveLocation
            "SCHEMA",
            "SCALAR",
            "OBJECT",
            "FIELD_DEFINITION",
            "ARGUMENT_DEFINITION",
            "INTERFACE",
            "UNION",
            "ENUM",
            "ENUM_VALUE",
            "INPUT_OBJECT",
            "INPUT_FIELD_DEFINITION",
        };

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

    private void Expect(TokenKind kind, string? description = null)
    {
        if (_currentToken.Kind == kind)
        {
            Advance();
        }
        else
        {
            Throw_From_Expect(kind, description);
        }
    }

    private void Throw_From_Expect(TokenKind kind, string? description = null)
    {
        throw new GraphQLSyntaxErrorException($"Expected {Token.GetTokenKindDescription(kind)}, found {_currentToken}{description}", _source, _currentToken.Start);
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
        throw new GraphQLSyntaxErrorException($"Expected \"{keyword}\", found {_currentToken}", _source, _currentToken.Start);
    }

    private string ExpectOneOf(string[] oneOf, bool advance = true)
    {
        if (_currentToken.Kind == TokenKind.NAME)
        {
            var found = IsAny(_currentToken, oneOf);
            if (found != null)
            {
                if (advance)
                    Advance();
                return found;
            }
        }

        return Throw_From_ExpectOneOf(oneOf);

        static string? IsAny(Token token, string[] oneOf)
        {
            foreach (string item in oneOf)
                if (token.Value == item)
                    return item;

            return null;
        }
    }

    private string Throw_From_ExpectOneOf(string[] oneOf)
    {
        throw new GraphQLSyntaxErrorException($"Expected \"{string.Join("/", oneOf)}\", found {_currentToken}", _source, _currentToken.Start);
    }
}
