using Microsoft.CodeAnalysis;

namespace UIComponents.Roslyn.Common.Readers
{
    /// <summary>
    /// Attribute readers fetch attributes from syntax nodes.
    /// </summary>
    public abstract class AttributeReader<TSyntaxNode, TData>
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
