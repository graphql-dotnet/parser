using System;
using System.Buffers;
using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.Document"/>.
    /// </summary>
    public class GraphQLDocument : ASTNode, IDisposable
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Document;

        // In some cases, the parser is forced to change the text (escape symbols, comments),
        // so it is impossible to simply point to the desired section (span) of the source text.
        // In this case, array pools are used, memory from which then need to be returned to the pool.
        internal List<(IMemoryOwner<char> owner, ASTNode rentedBy)>? RentedMemoryTracker { get; set; }

        public List<ASTNode>? Definitions { get; set; }

        /// <summary>
        /// Comments that have not been correlated to any AST node of GraphQL document.
        /// </summary>
        public List<GraphQLComment>? UnattachedComments { get; set; }

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
            GC.SuppressFinalize(this);
        }
    }

    internal sealed class GraphQLDocumentWithLocation : GraphQLDocument
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }
}
