# GraphQL.NET Parser

[![License](https://img.shields.io/github/license/graphql-dotnet/parser)](LICENSE.md)
[![codecov](https://codecov.io/gh/graphql-dotnet/parser/branch/master/graph/badge.svg?token=GEjwg1by60)](https://codecov.io/gh/graphql-dotnet/parser)
[![Nuget](https://img.shields.io/nuget/dt/GraphQL-Parser)](https://www.nuget.org/packages/GraphQL-Parser)
[![Nuget](https://img.shields.io/nuget/v/GraphQL-Parser)](https://www.nuget.org/packages/GraphQL-Parser)
[![GitHub Release Date](https://img.shields.io/github/release-date/graphql-dotnet/parser?label=released)](https://github.com/graphql-dotnet/parser/releases)
[![GitHub commits since latest release (by date)](https://img.shields.io/github/commits-since/graphql-dotnet/parser/latest?label=new+commits)](https://github.com/graphql-dotnet/parser/commits/master)
[![GitHub contributors](https://img.shields.io/github/contributors/graphql-dotnet/parser)](https://github.com/graphql-dotnet/parser/graphs/contributors)
![Size](https://img.shields.io/github/repo-size/graphql-dotnet/parser)

This library contains a lexer and parser as well as the complete [GraphQL AST model](http://spec.graphql.org/October2021/#sec-Appendix-Grammar-Summary)
that allows you to work with GraphQL documents compatible with the [October 2021 spec](https://spec.graphql.org/October2021/).

The parser from this library is used by the [GraphQL.NET](https://github.com/graphql-dotnet/graphql-dotnet) project
and was [verified](https://codecov.io/gh/graphql-dotnet/parser) by many test data sets.

Preview versions of this package are available on [GitHub Packages](https://github.com/orgs/graphql-dotnet/packages?repo_name=parser).

## 1. Lexer

Generates token based on input text. Lexer takes advantage of `ReadOnlyMemory<char>` and in most cases
does not allocate memory on the managed heap at all.

### Usage

```csharp
var token = Lexer.Lex("\"str\"");
```

Lex method always returns the first token it finds. In this case case the result would look like following.
![lexer example](assets/lexer-example.png)

## 2. Parser

Parses provided GraphQL expression into AST (abstract syntax tree). Parser also takes advantage of
`ReadOnlyMemory<char>` but still allocates memory for AST.

### Usage

```csharp
var ast1 = Parser.Parse(@"
{
  field
}");

var ast2 = Parser.Parse(@"
{
  field
}", new ParserOptions { Ignore = IgnoreOptions.Comments });
```

By default `ParserOptions.Ignore` is `IgnoreOptions.None`. If you want
to ignore all comments use `IgnoreOptions.Comments`. If you don't need
information about tokens locations in the source document, then use flag
`IgnoreOptions.Locations`. Or just use `IgnoreOptions.All` and this
will maximize the saving of memory allocated in the managed heap for AST.

## 3. ASTVisitor

`ASTVisitor` provides API to traverse AST of the parsed GraphQL document.
Default implementation traverses all AST nodes of the provided one. You can
inherit from it and override desired methods to implement your own AST
processing algorithm.

### SDLPrinter

For printing SDL from AST, you can use `SDLPrinter`. This is a highly
optimized visitor for asynchronous non-blocking SDL output into provided
`TextWriter`. In the majority of cases it does not allocate memory in
the managed heap at all. Extension methods are also provided for printing
directly to a string, which utilize the `StringBuilder` and `StringWriter`
classes.

```csharp
var document = Parser.Parse("query { hero { name age } }");

// print to a string with default options
var sdl = new SDLPrinter().Print(document);

// print to a string builder
var sb = new StringBuilder();
new SDLPrinter().Print(document, sb);

// print to a string with some options
var sdlPrinter = new SDLPrinter(
    new SDLPrinterOptions {
        PrintComments = true,
        EachDirectiveLocationOnNewLine = true,
        EachUnionMemberOnNewLine = true,
    });
var sdl = sdlPrinter.Print(document);

// print to a stream asynchronously
using var writer = new StreamWriter(stream);
await sdlPrinter.PrintAsync(document, writer, default);
await writer.FlushAsync();
```

Output:

```graphql
query {
  hero {
    name
    age
  }
}
```

### SDLSorter

An AST document can be sorted with the `SDLSorter` using a predefined
sort order.  You can specify the string comparison; by default it uses
a culture-invariant case-insensitive comparison.  Any futher customization
is possible by deriving from `SDLSorterOptions` and overriding the `Compare`
methods.

```csharp
var document = Parser.Parse("query { hero { name age } }");
SDLSorter.Sort(document);
var sdl = new SDLPrinter().Print(document);
```

Output:

```graphql
query {
  hero {
    age
    name
  }
}
```

### StructurePrinter

You can also find a `StructurePrinter` visitor that prints AST into the
provided `TextWriter` as a hierarchy of node types. It can be useful
when debugging for better understanding the AST structure.
Consider the following GraphQL document:

```graphql
query a { name age }
```

After `StructurePrinter` processing the output text will be

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

Usage:

```csharp
public static async Task PrintStructure(string sdl)
{
    var document = Parser.Parse(sdl);
    using var writer = new StringWriter(); 
    var printer = new StructurePrinter()
    await printer.PrintAsync(document, writer);
    var rendered = writer.ToString();
    Console.WriteLine(rendered);
}
```

## Contributors

This project exists thanks to all the people who contribute. 
<a href="https://github.com/graphql-dotnet/parser/graphs/contributors"><img src="https://contributors-img.web.app/image?repo=graphql-dotnet/parser" /></a>

PRs are welcome! Looking for something to work on? The list of [open issues](https://github.com/graphql-dotnet/parser/issues)
is a great place to start. You can help the project by simply responding to some of the [asked questions](https://github.com/graphql-dotnet/parser/issues?q=is%3Aissue+is%3Aopen+label%3Aquestion).
