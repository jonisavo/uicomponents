using Microsoft.CodeAnalysis;

namespace UIComponents.Roslyn.Generation.Readers
{
    /// <summary>
    /// Attribute readers fetch attributes from syntax nodes.
    /// </summary>
    internal abstract class AttributeReader<TSyntaxNode, TData> : IReader<TSyntaxNode, TData>
        where TSyntaxNode : SyntaxNode
    {
        protected readonly INamedTypeSymbol AttributeSymbol;
        protected readonly SemanticModel SemanticModel;

        public AttributeReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel)
        {
            AttributeSymbol = attributeSymbol;
            SemanticModel = semanticModel;
        }

        public abstract void Read(TSyntaxNode syntaxNode, TData output);

        protected bool NodeHasDesiredAttribute(AttributeData attributeData)
        {
            return attributeData.AttributeClass.Equals(AttributeSymbol, SymbolEqualityComparer.Default);
        }
    }
}
