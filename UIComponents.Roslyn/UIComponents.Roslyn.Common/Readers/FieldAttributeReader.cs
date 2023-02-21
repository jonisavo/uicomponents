using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace UIComponents.Roslyn.Common.Readers
{
    public sealed class FieldAttributeReader : AttributeReader<FieldDeclarationSyntax, IList<IFieldSymbol>>
    {
        public FieldAttributeReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public override void Read(FieldDeclarationSyntax fieldSyntax, IList<IFieldSymbol> output)
        {
            foreach (var variableDeclaration in fieldSyntax.Declaration.Variables)
            {
                var fieldSymbol = SemanticModel.GetDeclaredSymbol(variableDeclaration) as IFieldSymbol;

                var fieldAttributes = fieldSymbol.GetAttributes();

                if (fieldAttributes.Any(NodeHasDesiredAttribute))
                    output.Add(fieldSymbol);
            }
        }
    }
}
