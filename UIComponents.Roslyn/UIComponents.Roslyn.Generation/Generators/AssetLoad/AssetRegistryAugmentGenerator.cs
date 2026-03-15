using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Common.Utilities;
using UIComponents.Roslyn.Generation.SyntaxReceivers;

namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    [Generator]
    public sealed class AssetRegistryAugmentGenerator : ISourceGenerator
    {
        private struct ComponentEntry
        {
            public string FullyQualifiedName;
            public string LayoutPath;
            public List<string> StylesheetPaths;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ClassSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is ClassSyntaxReceiver receiver))
                return;

            var compilation = context.Compilation;

            var uiComponentSymbol = compilation.GetTypeByMetadataName("UIComponents.UIComponent");
            if (uiComponentSymbol == null)
                return;

            var layoutAttributeSymbol = compilation.GetTypeByMetadataName("UIComponents.LayoutAttribute");
            var stylesheetAttributeSymbol = compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");
            var sharedStylesheetAttributeSymbol = compilation.GetTypeByMetadataName("UIComponents.SharedStylesheetAttribute");
            var assetPrefixAttributeSymbol = compilation.GetTypeByMetadataName("UIComponents.AssetPrefixAttribute");
            var assetRootAttributeSymbol = compilation.GetTypeByMetadataName("UIComponents.AssetRootAttribute");

            var entries = new List<ComponentEntry>();

            foreach (var classDecl in receiver.GetTypes())
            {
                var semanticModel = compilation.GetSemanticModel(classDecl.SyntaxTree);
                var typeSymbol = semanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;

                if (typeSymbol == null)
                    continue;

                if (typeSymbol.IsAbstract)
                    continue;

                if (!IsAccessibleFromGeneratedCode(typeSymbol))
                    continue;

                if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentSymbol))
                    continue;

                var prefix = GetPrefix(typeSymbol, assetRootAttributeSymbol, assetPrefixAttributeSymbol, uiComponentSymbol);

                var layoutPath = FindLayoutPath(typeSymbol, layoutAttributeSymbol, uiComponentSymbol, prefix);
                var stylesheetPaths = CollectStylesheetPaths(
                    typeSymbol, stylesheetAttributeSymbol, sharedStylesheetAttributeSymbol,
                    uiComponentSymbol, prefix);

                if (layoutPath == null && stylesheetPaths.Count == 0)
                    continue;

                entries.Add(new ComponentEntry
                {
                    FullyQualifiedName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    LayoutPath = layoutPath,
                    StylesheetPaths = stylesheetPaths
                });
            }

            if (entries.Count == 0)
                return;

            var source = GenerateRegistrySource(entries);
            context.AddSource("UIComponentAssetRegistry.g.cs",
                SourceText.From(source, Encoding.UTF8));
        }

        /// <summary>
        /// Checks whether the type can be referenced from generated top-level code.
        /// Private or protected nested types are not accessible.
        /// </summary>
        private static bool IsAccessibleFromGeneratedCode(INamedTypeSymbol typeSymbol)
        {
            var current = typeSymbol;
            while (current != null)
            {
                switch (current.DeclaredAccessibility)
                {
                    case Accessibility.Private:
                    case Accessibility.Protected:
                    case Accessibility.ProtectedAndInternal:
                        return false;
                }
                current = current.ContainingType;
            }
            return true;
        }

        private static string GetPrefix(
            INamedTypeSymbol typeSymbol,
            INamedTypeSymbol assetRootAttributeSymbol,
            INamedTypeSymbol assetPrefixAttributeSymbol,
            INamedTypeSymbol uiComponentSymbol)
        {
            // Walk hierarchy current-first, prefer AssetRoot over AssetPrefix
            string assetRoot = null;
            string assetPrefix = null;

            var current = typeSymbol;
            while (current != null &&
                   !SymbolEqualityComparer.Default.Equals(current, uiComponentSymbol?.BaseType))
            {
                foreach (var attr in current.GetAttributes())
                {
                    if (assetRoot == null &&
                        assetRootAttributeSymbol != null &&
                        SymbolEqualityComparer.Default.Equals(attr.AttributeClass, assetRootAttributeSymbol))
                    {
                        if (attr.ConstructorArguments.Length > 0 &&
                            attr.ConstructorArguments[0].Value is string rootPath)
                            assetRoot = rootPath;
                    }

                    if (assetPrefix == null &&
                        assetPrefixAttributeSymbol != null &&
                        SymbolEqualityComparer.Default.Equals(attr.AttributeClass, assetPrefixAttributeSymbol))
                    {
                        if (attr.ConstructorArguments.Length > 0 &&
                            attr.ConstructorArguments[0].Value is string prefixPath)
                            assetPrefix = prefixPath;
                    }
                }

                if (assetRoot != null && assetPrefix != null)
                    break;

                current = current.BaseType;
            }

            return assetRoot ?? assetPrefix;
        }

        private static string BuildPrefixedPath(string path, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return path;

            return prefix + path;
        }

        /// <summary>
        /// Walks the type hierarchy current-first to find the most-derived
        /// [Layout] attribute. Returns the resolved path or null.
        /// </summary>
        private static string FindLayoutPath(
            INamedTypeSymbol typeSymbol,
            INamedTypeSymbol layoutAttributeSymbol,
            INamedTypeSymbol uiComponentSymbol,
            string prefix)
        {
            if (layoutAttributeSymbol == null)
                return null;

            var current = typeSymbol;

            while (current != null &&
                   !SymbolEqualityComparer.Default.Equals(current, uiComponentSymbol?.BaseType))
            {
                var layoutAttr = current.GetAttributes().FirstOrDefault(a =>
                    SymbolEqualityComparer.Default.Equals(a.AttributeClass, layoutAttributeSymbol));

                if (layoutAttr != null)
                {
                    var assetPath = layoutAttr.ConstructorArguments.Length > 0 &&
                        layoutAttr.ConstructorArguments[0].Value is string path
                            ? path
                            : current.Name;

                    return BuildPrefixedPath(assetPath, prefix);
                }

                current = current.BaseType;
            }

            return null;
        }

        /// <summary>
        /// Walks the type hierarchy base-first and collects all stylesheet paths
        /// from both [Stylesheet] and [SharedStylesheet] attributes.
        /// </summary>
        private static List<string> CollectStylesheetPaths(
            INamedTypeSymbol typeSymbol,
            INamedTypeSymbol stylesheetAttributeSymbol,
            INamedTypeSymbol sharedStylesheetAttributeSymbol,
            INamedTypeSymbol uiComponentSymbol,
            string prefix)
        {
            var paths = new List<string>();

            // Build base-first hierarchy
            var typeHierarchy = new List<INamedTypeSymbol>();
            var current = typeSymbol;

            while (current != null &&
                   !SymbolEqualityComparer.Default.Equals(current, uiComponentSymbol?.BaseType))
            {
                typeHierarchy.Add(current);
                current = current.BaseType;
            }

            typeHierarchy.Reverse();

            foreach (var type in typeHierarchy)
            {
                var attributes = type.GetAttributes();

                // Collect [Stylesheet] attributes for this type
                if (stylesheetAttributeSymbol != null)
                {
                    foreach (var attribute in attributes)
                    {
                        if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, stylesheetAttributeSymbol))
                            continue;

                        if (attribute.ConstructorArguments.Length > 0 &&
                            attribute.ConstructorArguments[0].Value is string path)
                            paths.Add(BuildPrefixedPath(path, prefix));
                        else
                            paths.Add(BuildPrefixedPath(type.Name + Constants.ConventionStylesheetSuffix, prefix));
                    }
                }

                // Collect [SharedStylesheet] attributes for this type
                if (sharedStylesheetAttributeSymbol != null)
                {
                    foreach (var attribute in attributes)
                    {
                        if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, sharedStylesheetAttributeSymbol))
                            continue;

                        if (attribute.ConstructorArguments.Length > 0 &&
                            attribute.ConstructorArguments[0].Value is string name)
                            paths.Add(BuildPrefixedPath(name, prefix));
                    }
                }
            }

            return paths;
        }

        private static string GenerateRegistrySource(List<ComponentEntry> entries)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
");

            sb.AppendLine(Constants.GeneratedCodeAttribute);
            sb.AppendLine(@"internal static class UIComponentAssetRegistry
{
    public struct AssetEntry
    {
        public string LayoutPath;
        public string[] StylesheetPaths;
    }

    private static readonly Dictionary<Type, AssetEntry> Entries = new Dictionary<Type, AssetEntry>
    {");

            foreach (var entry in entries)
            {
                var layoutValue = entry.LayoutPath != null
                    ? $"\"{entry.LayoutPath}\""
                    : "null";

                string stylesheetValue;
                if (entry.StylesheetPaths.Count > 0)
                {
                    var paths = string.Join(", ", entry.StylesheetPaths.Select(p => $"\"{p}\""));
                    stylesheetValue = $"new[] {{ {paths} }}";
                }
                else
                {
                    stylesheetValue = "new string[0]";
                }

                sb.AppendLine($"        {{ typeof({entry.FullyQualifiedName}), new AssetEntry {{ LayoutPath = {layoutValue}, StylesheetPaths = {stylesheetValue} }} }},");
            }

            sb.AppendLine(@"    };

    public static bool TryGetEntry(Type componentType, out AssetEntry entry)
    {
        return Entries.TryGetValue(componentType, out entry);
    }
}");

            return sb.ToString();
        }
    }
}
