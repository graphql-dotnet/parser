using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors
{
    public class SDLWriterTests
    {
        private class TestContext : IWriteContext
        {
            public TextWriter Writer { get; set; } = new StringWriter();

            public Stack<AST.ASTNode> Parent { get; set; } = new Stack<AST.ASTNode>();
        }

        private static readonly SDLWriter<TestContext> _sdlWriter = new();

        [Theory]
        [InlineData(@"#comment
input Example @x {
  self: [Example!]!
  value: String = ""xyz""
}
input B
input C
",
@"#comment
input Example @x
{
  self: [Example!]!
  value: String = ""xyz""
}

input B

input C
")]
        [InlineData(@"query inlineFragmentTyping {
  profiles(handles: [""zuck"", ""coca - cola""]) {
    handle
    ... on User {
      friends {
        count
      }
}
... on Page
{
    likers
    {
        count
      }
}
  }
}
", @"query inlineFragmentTyping
{
  profiles(handles: [""zuck"", ""coca - cola""])
  {
    handle
    ... on User
    {
      friends
      {
        count
      }
    }
    ... on Page
    {
      likers
      {
        count
      }
    }
  }
}
")]
        [InlineData(@"scalar a scalar b scalar c", @"scalar a

scalar b

scalar c
")]
        [InlineData(@"{
  foo
    #comment on fragment
  ...Frag
  qux
}

fragment Frag on Query {
  bar
  baz
}
",

@"
{
  foo
  #comment on fragment
  ...Frag
  qux
}

fragment Frag on Query
{
  bar
  baz
}
")]
        [InlineData(@"union Animal @immutable = |Cat | Dog", @"union Animal @immutable = Cat | Dog")]
        [InlineData(@"schema @checked { mutation: MyMutation subscription: MySub }", @"schema @checked
{
  mutation: MyMutation
  subscription: MySub
}
")]
        [InlineData(@"interface Dog implements & Eat & Bark { volume: Int! }",
            @"interface Dog implements Eat & Bark
{
  volume: Int!
}
")]
        [InlineData(@"enum Color { RED,
#good color
GREEN @directive(list: [1,2,3,null,{}, {name:""tom"" age:42}]),
""""""
another good color
""""""
BLUE }", @"enum Color
{
  RED
  #good color
  GREEN @directive(list: [1, 2, 3, null, { }, { name: ""tom"", age: 42 }])
  """"""
  another good color
  """"""
  BLUE
}
")]
        [InlineData(@"# super query
#
# multiline
query summary($id: ID!) { name(full:true,kind: UPPER) age1:age address { street @short(length:5,x:""a"", pi: 3.14)
#need
building } }",

@"# super query
#
# multiline
query summary($id: ID!)
{
  name(full: true, kind: UPPER)
  age1: age
  address
  {
    street @short(length: 5, x: ""a"", pi: 3.14)
    #need
    building
  }
}
")]
        [InlineData(@"
""""""
  description
    indent 2
      indent4
""""""
scalar JSON @exportable
# A dog
type Dog implements &Animal {
  """"""inline docs""""""
  volume: Float
  """"""
 multiline
 docs
  """"""
  friends: [Dog!]
  age: Int!
}",

@"""""""
description
  indent 2
    indent4
""""""
scalar JSON @exportable

# A dog
type Dog implements Animal
{
  """"""
  inline docs
  """"""
  volume: Float
  """"""
  multiline
  docs
  """"""
  friends: [Dog!]
  age: Int!
}
")]
        public async Task WriteDocumentVisitor_Should_Print_Document(string text, string expected)
        {
            var context = new TestContext();

            using (var document = text.Parse())
            {
                await _sdlWriter.Visit(document, context);
                var actual = context.Writer.ToString();
                actual.ShouldBe(expected);

                using (var parsedBack = actual.Parse())
                {
                    // should be parsed back
                }
            }
        }
    }
}
