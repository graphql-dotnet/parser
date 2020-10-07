using System.IO;
using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class LexerBenchmark
    {
        private string query; 
        private const string KITCHEN_SINK = @"
query queryName($foo: ComplexType, $site: Site = MOBILE) {
  whoever123is: node(id: [123, 456]) {
    id ,
    ... on User @defer {
      field2 {
        id ,
        alias: field1(first:10, after:$foo,) @include(if: $foo) {
          id,
          ...frag
        }
      }
    }
    ... @skip(unless: $foo) {
      id
    }
    ... {
      id
    }
  }
}

mutation likeStory {
  like(story: 123) @defer {
    story {
      id
    }
  }
}

subscription StoryLikeSubscription($input: StoryLikeSubscribeInput) {
  storyLikeSubscribe(input: $input) {
    story {
      likers {
        count
      }
      likeSentence {
        text
      }
    }
  }
}

fragment frag on Friend {
  foo(size: $size, bar: $b, obj: {key: ""value""})
}

{
  unnamed(truthy: true, falsey: false),
  query
}";

        [GlobalSetup]
        public void GlobalSetup()
        {
            query = File.ReadAllText("query_with_many_escape_symbols.txt");
        }

        [Benchmark]
        public void LexKitchenSink()
        {
            var lexer = new Lexer();
            var source = new Source(KITCHEN_SINK);
            var resetPosition = 0;
            Token token;
            while ((token = lexer.Lex(source, resetPosition)).Kind != TokenKind.EOF)
            {
                resetPosition = token.End;
            }
        }

        [Benchmark]
        public void LexQueryWithManyEscapeSymbols()
        {
            var lexer = new Lexer();
            var source = new Source(query);
            var resetPosition = 0;
            Token token;
            while ((token = lexer.Lex(source, resetPosition)).Kind != TokenKind.EOF)
            {
                resetPosition = token.End;
            }
        }
    }
}
