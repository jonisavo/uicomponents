using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators
{
    public struct AugmentGenerationContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext;
        public ClassDeclarationSyntax ClassSyntax;
        public SemanticModel ClassSemanticModel;
        public string TypeName;
        public string CurrentTypeNamespace;
        public INamedTypeSymbol CurrentTypeSymbol;

        public string GetTypeName(ITypeSymbol type)
        {
            return RoslynUtilities.GetTypeNameWithoutRootNamespace(type, CurrentTypeNamespace);
        }
    }
}
