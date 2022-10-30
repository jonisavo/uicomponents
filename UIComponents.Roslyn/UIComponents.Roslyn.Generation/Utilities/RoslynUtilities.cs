using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.Utilities
{
    public static class RoslynUtilities
    {
        // https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
        public static string GetTypeNamespace(SyntaxNode syntaxNode)
        {
            var currentNamespace = string.Empty;
            var potentialNamespaceParent = GetNamespaceDeclaration(syntaxNode);

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

        public static NamespaceDeclarationSyntax GetNamespaceDeclaration(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
                return null;

            var potentialNamespaceParent = syntaxNode.Parent;

            while (potentialNamespaceParent != null && !(potentialNamespaceParent is NamespaceDeclarationSyntax))
                potentialNamespaceParent = potentialNamespaceParent.Parent;

            return potentialNamespaceParent as NamespaceDeclarationSyntax;
        }

        public static string GetTypeNameWithoutRootNamespace(ITypeSymbol type, string nameSpace)
        {
            var displayString = type.ToDisplayString();

            var namespaceParts = nameSpace.Split('.');

            if (namespaceParts.Length < 2)
                return displayString;

            var rootNamespace = namespaceParts[0];

            if (!displayString.StartsWith(rootNamespace))
                return displayString;

            var firstDotIndex = displayString.IndexOf('.') + 1;

            return displayString.Substring(firstDotIndex);
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

        public static bool HasBaseType(INamedTypeSymbol type, INamedTypeSymbol desiredBaseType)
        {
            var current = type;

            while (current != null)
            {
                if (current.Equals(desiredBaseType, SymbolEqualityComparer.Default))
                    return true;

                current = current.BaseType;
            }

            return false;
        }

        public static void ReadAttributeArguments(AttributeData attributeData, Dictionary<string, TypedConstant> output)
        {
            var constructorArgs = attributeData.ConstructorArguments;

            for (var i = 0; i < constructorArgs.Length; i++)
                output.Add($"constructor_{i}", constructorArgs[i]);

            foreach (var argument in attributeData.NamedArguments)
                output.Add(argument.Key, argument.Value);
        }

        public static ITypeSymbol GetMemberType(ISymbol symbol)
        {
            if (symbol is IFieldSymbol fieldSymbol)
                return fieldSymbol.Type;
            else if (symbol is IPropertySymbol propertySymbol)
                return propertySymbol.Type;

            return null;
        }

        public static ITypeSymbol GetConcreteType(ITypeSymbol symbol)
        {
            if (symbol is IArrayTypeSymbol arrayTypeSymbol)
                return arrayTypeSymbol.ElementType;

            if (symbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
                return namedTypeSymbol.TypeArguments[0];

            return symbol;
        }

        public static IEnumerable<ISymbol> GetAllMembersOfType(ITypeSymbol typeSymbol)
        {
            var current = typeSymbol;

            while (current != null)
            {
                foreach (var member in current.GetMembers())
                    yield return member;

                current = current.BaseType;
            }
        }

        public static IEnumerable<AttributeData> GetAllAttributesOfType(ITypeSymbol typeSymbol)
        {
            var current = typeSymbol;

            while (current != null)
            {
                foreach (var attribute in current.GetAttributes())
                    yield return attribute;

                current = current.BaseType;
            }
        }
    }
}
