using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace UIComponents.Roslyn.Generation.Readers
{
    public sealed class PropertyAttributeReader : AttributeReader<PropertyDeclarationSyntax, IList<IPropertySymbol>>
    {
        public PropertyAttributeReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public override void Read(PropertyDeclarationSyntax propertySyntax, IList<IPropertySymbol> output)
        {
            var propertySymbol = SemanticModel.GetDeclaredSymbol(propertySyntax) as IPropertySymbol;

            if (propertySymbol.SetMethod == null)
                return;

            var propertyAttributes = propertySymbol.GetAttributes();

            if (propertyAttributes.Any(NodeHasDesiredAttribute))
                output.Add(propertySymbol);
        }
    }
}
