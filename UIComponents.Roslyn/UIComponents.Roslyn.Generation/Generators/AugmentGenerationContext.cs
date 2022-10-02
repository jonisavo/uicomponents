using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UIComponents.Roslyn.Generation.Generators
{
    internal struct AugmentGenerationContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext;
        public ClassDeclarationSyntax ClassSyntax;
        public SemanticModel ClassSemanticModel;
        public string TypeName;
        public string CurrentTypeNamespace;
        public INamedTypeSymbol CurrentTypeSymbol;
    }
}
