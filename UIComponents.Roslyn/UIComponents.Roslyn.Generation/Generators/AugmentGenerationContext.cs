﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators
{
    public struct AugmentGenerationContext
    {
        public GeneratorExecutionContext GeneratorExecutionContext;
        public TypeDeclarationSyntax ClassSyntax;
        public SemanticModel ClassSemanticModel;
        public string TypeName;
        public string CurrentTypeNamespace;
        public INamedTypeSymbol CurrentTypeSymbol;

        public string GetTypeName(ITypeSymbol type)
        {
            var typeName = CurrentTypeSymbol.ToDisplayString();
            return RoslynUtilities.GetTypeNameForNamespace(type, typeName);
        }
    }
}
