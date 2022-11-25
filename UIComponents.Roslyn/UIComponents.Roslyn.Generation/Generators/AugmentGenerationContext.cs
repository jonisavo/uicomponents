using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
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
        public HashSet<string> Usings;

        public string GetTypeName(ITypeSymbol type)
        {
            var typeName = type.ToDisplayString();

            // Sort namespaces declared with usings, with longest first.
            // There may be a situation where we have type like
            // UnityEngine.UIElements.VisualElement and two usings,
            // "using UnityEngine;" and "using UnityEngine.UIElements;".
            // We want to shorten using the longest namespace.
            var usingsSortedByLength = Usings.ToList();
            usingsSortedByLength.Sort((first, second) => second.Length - first.Length);

            foreach (var namespaceName in usingsSortedByLength)
            {
                // We want to keep UIComponents' type names explicit.
                if (namespaceName.StartsWith("UIComponents"))
                    continue;

                var namespaceNameWithTrailingDot = namespaceName + ".";

                if (typeName.StartsWith(namespaceNameWithTrailingDot))
                    return typeName.Substring(namespaceNameWithTrailingDot.Length);
            }

            var currentTypeName = CurrentTypeSymbol.ToDisplayString();

            return RoslynUtilities.GetTypeNameForNamespace(type, currentTypeName);
        }
    }
}
