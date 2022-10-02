using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UIComponents.Roslyn.Generation.Utilities
{
    internal static class CodeAnalyzingUtilities
    {
        // https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
        public static string GetTypeNamespace(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
                return string.Empty;

            var currentNamespace = string.Empty;

            var potentialNamespaceParent = syntaxNode.Parent;

            while (potentialNamespaceParent != null && !(potentialNamespaceParent is NamespaceDeclarationSyntax))
                potentialNamespaceParent = potentialNamespaceParent.Parent;

            if (potentialNamespaceParent is NamespaceDeclarationSyntax namespaceParent)
            {
                currentNamespace = namespaceParent.Name.ToString();

                while (true)
                {
                    if (!(namespaceParent.Parent is NamespaceDeclarationSyntax parent))
                        break;

                    currentNamespace = $"{parent.Name}.{currentNamespace}";
                    namespaceParent = parent;
                }
            }

            return currentNamespace;
        }

        public static BaseTypeDeclarationSyntax GetBaseTypeSyntax(SyntaxNode syntaxNode)
        {
            while (syntaxNode != null)
            {
                if (syntaxNode is BaseTypeDeclarationSyntax typeDeclarationSyntax)
                    return typeDeclarationSyntax;

                syntaxNode = syntaxNode.Parent;
            }

            return null;
        }

        public static string GetTypeName(SyntaxNode syntaxNode)
        {
            var baseTypeSyntaxNode = GetBaseTypeSyntax(syntaxNode);

            if (baseTypeSyntaxNode == null)
                return string.Empty;

            return baseTypeSyntaxNode.Identifier.ValueText;
        }
    }
}
