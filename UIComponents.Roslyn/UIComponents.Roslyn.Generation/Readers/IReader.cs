using Microsoft.CodeAnalysis;

namespace UIComponents.Roslyn.Generation.Readers
{
    /// <summary>
    /// An interface for a construct that fetches data from syntax nodes.
    /// </summary>
    /// <typeparam name="TSyntaxNode">Syntax node type</typeparam>
    /// <typeparam name="TData">Data type</typeparam>
    public interface IReader<TSyntaxNode, TData> where TSyntaxNode : SyntaxNode
    {
        /// <summary>
        /// Navigates the syntax node looking for data.
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="output">Data output</param>
        void Read(TSyntaxNode syntaxNode, TData output);
    }
}
