using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLObjectValue : GraphQLValue
    {
        public List<GraphQLObjectField>? Fields { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.ObjectValue;
    }
}