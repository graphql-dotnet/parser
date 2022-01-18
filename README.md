# GraphQL Dotnet Parser

[![Publish release to Nuget registry](https://github.com/graphql-dotnet/parser/actions/workflows/publish-release.yml/badge.svg)](https://github.com/graphql-dotnet/parser/actions/workflows/publish-release.yml)
[![Publish preview to GitHub registry](https://github.com/graphql-dotnet/parser/actions/workflows/publish-preview.yml/badge.svg)](https://github.com/graphql-dotnet/parser/actions/workflows/publish-preview.yml)

[![Run unit tests](https://github.com/graphql-dotnet/parser/actions/workflows/test.yml/badge.svg)](https://github.com/graphql-dotnet/parser/actions/workflows/test.yml)
[![CodeQL analysis](https://github.com/graphql-dotnet/parser/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/graphql-dotnet/parser/actions/workflows/codeql-analysis.yml)
[![codecov](https://codecov.io/gh/graphql-dotnet/parser/branch/master/graph/badge.svg?token=GEjwg1by60)](https://codecov.io/gh/graphql-dotnet/parser)

[![NuGet](https://img.shields.io/nuget/v/GraphQL-Parser.svg)](https://www.nuget.org/packages/GraphQL-Parser)
[![Nuget](https://img.shields.io/nuget/dt/GraphQL-Parser)](https://www.nuget.org/packages/GraphQL-Parser)

![Activity](https://img.shields.io/github/commit-activity/w/graphql-dotnet/parser)
![Activity](https://img.shields.io/github/commit-activity/m/graphql-dotnet/parser)
![Activity](https://img.shields.io/github/commit-activity/y/graphql-dotnet/parser)

![Size](https://img.shields.io/github/repo-size/graphql-dotnet/parser)

This library contains a lexer and parser classes as well as the complete [GraphQL AST model](http://spec.graphql.org/October2021/#sec-Appendix-Grammar-Summary).

The parser from this library is used in [GraphQL for .NET](https://github.com/graphql-dotnet/graphql-dotnet).

Preview versions of this package are available on [GitHub Packages](https://github.com/orgs/graphql-dotnet/packages?repo_name=parser).

## 1. Lexer

Generates token based on input text. Lexer takes advantage of `ReadOnlyMemory<char>` and in most cases
does not allocate memory on the managed heap at all.

### Usage

```c#
var token = Lexer.Lex("\"str\"");
```

Lex method always returns the first token it finds. In this case case the result would look like following.
![lexer example](assets/lexer-example.png)

## 2. Parser

Parses provided GraphQL expression into AST (abstract syntax tree). Parser also takes advantage of
`ReadOnlyMemory<char>` but still allocates memory for AST.

### Usage

```c#
var ast1 = Parser.Parse(@"
{
  field
}");

var ast2 = Parser.Parse(@"
{
  field
}", new ParserOptions { Ignore = IgnoreOptions.IgnoreComments });
```

By default `ParserOptions.Ignore` is `IgnoreOptions.IgnoreComments` to improve performance.
If you don't need information about tokens locations in the source document, then use `IgnoreOptions.IgnoreCommentsAndLocations`.
This will maximize the saving of memory allocated in the managed heap for AST.

## 3. INodeVisitor

`INodeVisitor` provides API to traverse AST of the parsed GraphQL document.
Default implementation of this interface is `DefaultNodeVisitor` that
traverses all AST nodes of the provided one. You can inherit from it and
implement your own AST processing algorithm.

For printing SDL from AST, you can use `SDLWriter<TContext>` visitor.
This is a highly optimized visitor for asynchronous non-blocking SDL output
into provided `TextWriter`. In the majority of cases it does not allocate
memory in the managed heap at all.

You can also find a `StructureWriter<TContext>` visitor that prints AST
into the provided `TextWriter` as a hierarchy of node types. It can be useful
when debugging for better understanding the AST structure.
Consider GraphQL document

```graphql
query a { name age }
```

After `StructureWriter` processing the output text will be

```
Document
  OperationDefinition
    Name [a]
    SelectionSet
      Field
        Name [name]
      Field
        Name [age]
```

### Usage

```c#
public class Context : IWriteContext
{
    public TextWriter Writer { get; set; } = new StringWriter();

    public Stack<AST.ASTNode> Parents { get; set; } = new Stack<AST.ASTNode>();

    public CancellationToken CancellationToken { get; set; }

    public int IndentLevel { get; set; }
}

public static void Parse(string text)
{
    var document = Parser.Parse(text);

    var context = new Context();
    var visitor = new SDLWriter<Context>()
    await visitor.Visit(document, context);
    var rendered = context.Writer.ToString();
    Console.WriteLine(rendered);
}
```
