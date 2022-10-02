using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.Readers
{
    internal class PropertyAttributeArgumentReader : AttributeArgumentReader<PropertyDeclarationSyntax>
    {
        public PropertyAttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public override void Read(PropertyDeclarationSyntax syntaxNode, Dictionary<string, TypedConstant> output)
        {
            var propertySymbol = SemanticModel.GetDeclaredSymbol(syntaxNode) as IPropertySymbol;

            GetArgumentsOfSymbol(propertySymbol, output);
        }
    }
}
