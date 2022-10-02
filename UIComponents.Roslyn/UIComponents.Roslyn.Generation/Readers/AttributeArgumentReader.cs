using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace UIComponents.Roslyn.Generation.Readers
{
    internal abstract class AttributeArgumentReader<TSyntaxNode> : AttributeReader<TSyntaxNode, Dictionary<string, TypedConstant>>
        where TSyntaxNode : SyntaxNode
    {
        public AttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        protected void GetArgumentsOfSymbol(ISymbol symbol, Dictionary<string, TypedConstant> arguments)
        {
            var fieldAttributes = symbol.GetAttributes().Where(NodeHasDesiredAttribute);

            foreach (var attribute in fieldAttributes)
                foreach (var argument in attribute.NamedArguments)
                    arguments.Add(argument.Key, argument.Value);
        }
    }
}
