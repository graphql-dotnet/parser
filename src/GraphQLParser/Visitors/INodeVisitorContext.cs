using System.Threading;

namespace GraphQLParser.Visitors
{
    /// <summary>
    /// Context passed into all INodeVisitor.VisitXXX methods.
    /// </summary>
    public interface INodeVisitorContext
    {
        /// <summary>
        /// The token to monitor for cancellation requests.
        /// </summary>
        CancellationToken CancellationToken { get; }
    }
}
