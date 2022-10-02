using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.Readers
{
    internal class VariableAttributeArgumentReader : AttributeArgumentReader<VariableDeclaratorSyntax>
    {
        public VariableAttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public override void Read(VariableDeclaratorSyntax syntaxNode, Dictionary<string, TypedConstant> output)
        {
            var fieldSymbol = SemanticModel.GetDeclaredSymbol(syntaxNode) as IFieldSymbol;

            GetArgumentsOfSymbol(fieldSymbol, output);
        }
    }
}
