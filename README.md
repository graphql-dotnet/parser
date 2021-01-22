# GraphQL Dotnet Parser

[![AppVeyor](https://img.shields.io/appveyor/ci/graphql-dotnet-ci/parser.svg)](https://ci.appveyor.com/project/graphql-dotnet-ci/parser)
[![Coverage Status](https://coveralls.io/repos/github/graphql-dotnet/parser/badge.svg?branch=master)](https://coveralls.io/github/graphql-dotnet/parser?branch=master)
[![NuGet](https://img.shields.io/nuget/v/GraphQL-Parser.svg)](https://www.nuget.org/packages/GraphQL-Parser)
[![MyGet Pre Release](https://img.shields.io/myget/graphql-dotnet/vpre/GraphQL-Parser?label=myget)](https://www.myget.org/F/graphql-dotnet/api/v3/index.json)
[![Nuget](https://img.shields.io/nuget/dt/GraphQL-Parser)](https://www.nuget.org/packages/GraphQL-Parser)

![Activity](https://img.shields.io/github/commit-activity/w/graphql-dotnet/parser)
![Activity](https://img.shields.io/github/commit-activity/m/graphql-dotnet/parser)
![Activity](https://img.shields.io/github/commit-activity/y/graphql-dotnet/parser)

![Size](https://img.shields.io/github/repo-size/graphql-dotnet/parser)

This library contains a lexer and parser classes as well as the complete GraphQL AST model.

The parser from this library is used in [GraphQL for .NET](https://github.com/graphql-dotnet/graphql-dotnet).

## Lexer

Generates token based on input text. Lexer takes advantage of `ReadOnlyMemory<char>` and in most cases
does not allocate memory on the managed heap at all.

### Usage

```c#
var token = Lexer.Lex("\"str\"");
```

Lex method always returns the first token it finds. In this case case the result would look like following.
![lexer example](assets/lexer-example.png)

## Parser

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

### Example of json representation of the resulting AST

```json
{
  "Definitions": [{
    "Directives": [],
    "Kind": 2,
    "Name": null,
    "Operation": 0,
    "SelectionSet": {
      "Kind": 5,
      "Selections": [{
        "Alias": null,
        "Arguments": [],
        "Directives": [],
        "Kind": 6,
        "Name": {
          "Kind": 0,
          "Value": "field",
          "Location": {
            "End": 50,
            "Start": 31
          }
        },
        "SelectionSet": null,
        "Location": {
          "End": 50,
          "Start": 31
        }
      }],
      "Location": {
        "End": 50,
        "Start": 13
      }
    },
    "VariableDefinitions": null,
    "Location": {
      "End": 50,
      "Start": 13
    }
  }],
  "Kind": 1,
  "Location": {
    "End": 50,
    "Start": 13
  }
}
```
