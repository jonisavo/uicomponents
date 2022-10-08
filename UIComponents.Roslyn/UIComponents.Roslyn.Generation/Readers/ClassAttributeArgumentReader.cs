using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.Readers
{
    internal class ClassAttributeArgumentReader : AttributeArgumentReader<ClassDeclarationSyntax>
    {
        public ClassAttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public override void Read(ClassDeclarationSyntax syntaxNode, Dictionary<string, TypedConstant> output)
        {
            var fieldSymbol = SemanticModel.GetDeclaredSymbol(syntaxNode) as INamedTypeSymbol;

            GetArgumentsOfSymbol(fieldSymbol, output);
        }
    }
}
