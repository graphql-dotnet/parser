using System;
using System.Buffers;
using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents the root of AST (Abstract Syntax Tree) for GraphQL document.
    /// </summary>
    public class GraphQLDocument : ASTNode, IDisposable
    {
        // In some cases, the parser is forced to change the text (escape symbols, comments),
        // so it is impossible to simply point to the desired section (span) of the source text.
        // In this case, array pools are used, memory from which then need to be returned to the pool.
        internal List<(IMemoryOwner<char> owner, ASTNode rentedBy)>? RentedMemoryTracker { get; set; }

        public List<ASTNode>? Definitions { get; set; }

        public List<GraphQLComment>? UnattachedComments { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Document;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var temp = RentedMemoryTracker;
                if (temp != null)
                {
                    RentedMemoryTracker = null;
                    foreach (var (owner, rentedBy) in temp)
                    {
                        owner.Dispose();

                        // memory returned to the pool can no longer be used so for safety, we erase the reference to it from the node
                        if (rentedBy is GraphQLComment comment)
                            comment.Text = default;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }
    }

    internal sealed class GraphQLDocumentFull : GraphQLDocument
    {
        private GraphQLLocation _location;
        //private GraphQLComment? _comment;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        // TODO: this property is not set anywhere (yet), so it makes no sense to create a field for it
        //public override GraphQLComment? Comment
        //{
        //    get => _comment;
        //    set => _comment = value;
        //}
    }
}
