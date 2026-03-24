using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UIComponentIdenticalStylesheetAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC101";
        private const string ConventionStylesheetSuffix = ".style";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC101_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC101_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC101_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Layout";

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(startContext =>
            {
                var uiComponentTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
                var stylesheetAttributeTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");
                var sharedStylesheetAttributeTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.SharedStylesheetAttribute");

                if (uiComponentTypeSymbol != null && stylesheetAttributeTypeSymbol != null)
                {
                    startContext.RegisterSymbolAction(symbolContext =>
                            AnalyzeSymbol(
                                symbolContext,
                                uiComponentTypeSymbol,
                                stylesheetAttributeTypeSymbol,
                                sharedStylesheetAttributeTypeSymbol),
                        SymbolKind.NamedType);
                }
            });
        }

        private static void AnalyzeSymbol(
            SymbolAnalysisContext context,
            INamedTypeSymbol uiComponentTypeSymbol,
            INamedTypeSymbol stylesheetAttributeTypeSymbol,
            INamedTypeSymbol sharedStylesheetAttributeTypeSymbol)
        {
            var typeSymbol = (INamedTypeSymbol)context.Symbol;
            if (typeSymbol.TypeKind != TypeKind.Class)
                return;

            if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentTypeSymbol))
                return;

            var seenStylesheets = new HashSet<string>();

            foreach (var type in GetTypeHierarchy(typeSymbol, uiComponentTypeSymbol))
            {
                var isCurrentType = SymbolEqualityComparer.Default.Equals(type, typeSymbol);

                foreach (var attribute in GetAttributes(type, stylesheetAttributeTypeSymbol))
                {
                    var stylesheetPath = GetStylesheetPath(attribute, type.Name);
                    if (stylesheetPath == null)
                        continue;

                    if (!seenStylesheets.Add(stylesheetPath))
                    {
                        if (isCurrentType)
                            ReportDuplicateDiagnostic(context, attribute, stylesheetPath, typeSymbol.Name);

                        continue;
                    }
                }

                foreach (var attribute in GetAttributes(type, sharedStylesheetAttributeTypeSymbol))
                {
                    var stylesheetPath = GetExplicitAssetPath(attribute);
                    if (stylesheetPath == null)
                        continue;

                    if (!seenStylesheets.Add(stylesheetPath))
                    {
                        if (isCurrentType)
                            ReportDuplicateDiagnostic(context, attribute, stylesheetPath, typeSymbol.Name);

                        continue;
                    }
                }
            }
        }

        private static IEnumerable<INamedTypeSymbol> GetTypeHierarchy(
            INamedTypeSymbol typeSymbol,
            INamedTypeSymbol uiComponentTypeSymbol)
        {
            var hierarchy = new List<INamedTypeSymbol>();
            var current = typeSymbol;

            while (current != null &&
                   !SymbolEqualityComparer.Default.Equals(current, uiComponentTypeSymbol?.BaseType))
            {
                hierarchy.Add(current);
                current = current.BaseType;
            }

            hierarchy.Reverse();
            return hierarchy;
        }

        private static IEnumerable<AttributeData> GetAttributes(
            INamedTypeSymbol typeSymbol,
            INamedTypeSymbol attributeTypeSymbol)
        {
            if (attributeTypeSymbol == null)
                yield break;

            foreach (var attribute in typeSymbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeTypeSymbol))
                    yield return attribute;
            }
        }

        private static string GetStylesheetPath(AttributeData attribute, string declaringTypeName)
        {
            if (attribute.ConstructorArguments.Length == 0)
                return declaringTypeName + ConventionStylesheetSuffix;

            return GetExplicitAssetPath(attribute);
        }

        private static string GetExplicitAssetPath(AttributeData attribute)
        {
            if (attribute.ConstructorArguments.Length == 0)
                return null;

            if (!(attribute.ConstructorArguments[0].Value is string path))
                return null;

            return string.IsNullOrWhiteSpace(path) ? null : path;
        }

        private static void ReportDuplicateDiagnostic(
            SymbolAnalysisContext context,
            AttributeData attribute,
            string stylesheetPath,
            string componentName)
        {
            var attributeSyntax = attribute.ApplicationSyntaxReference?.GetSyntax(context.CancellationToken) as AttributeSyntax;
            var location = attributeSyntax?.GetLocation() ?? context.Symbol.Locations.FirstOrDefault();
            if (location == null)
                return;

            var diagnostic = Diagnostic.Create(Rule, location, stylesheetPath, componentName);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
