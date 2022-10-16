using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Readers
{
    internal class AttributeArgumentReader : AttributeReader<SyntaxNode, Dictionary<AttributeData, Dictionary<string, TypedConstant>>>
    {
        public AttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public override void Read(SyntaxNode syntaxNode, Dictionary<AttributeData, Dictionary<string, TypedConstant>> output)
        {
            var propertySymbol = SemanticModel.GetDeclaredSymbol(syntaxNode);

            GetArgumentsOfSymbol(propertySymbol, output);
        }

        protected void GetArgumentsOfSymbol(ISymbol symbol, Dictionary<AttributeData, Dictionary<string, TypedConstant>> attributeArgs)
        {
            var allAttributes = symbol.GetAttributes();
            var attributes = allAttributes.Where(NodeHasDesiredAttribute);

            foreach (var attribute in attributes)
            {
                var arguments = new Dictionary<string, TypedConstant>();
                attributeArgs.Add(attribute, arguments);
                RoslynUtilities.ReadAttributeArguments(attribute, arguments);
            }
        }
    }
}
