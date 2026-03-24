using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AssetRootAndPrefixConflictAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC105";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC105_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC105_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC105_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Layout";

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning,
                isEnabledByDefault: true, description: Description);

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
                var assetRootAttributeTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.AssetRootAttribute");
                var assetPrefixAttributeTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.AssetPrefixAttribute");

                if (uiComponentTypeSymbol != null &&
                    assetRootAttributeTypeSymbol != null &&
                    assetPrefixAttributeTypeSymbol != null)
                {
                    startContext.RegisterSymbolAction(symbolContext =>
                            AnalyzeSymbol(
                                symbolContext,
                                uiComponentTypeSymbol,
                                assetRootAttributeTypeSymbol,
                                assetPrefixAttributeTypeSymbol),
                        SymbolKind.NamedType);
                }
            });
        }

        private static void AnalyzeSymbol(
            SymbolAnalysisContext context,
            INamedTypeSymbol uiComponentTypeSymbol,
            INamedTypeSymbol assetRootAttributeTypeSymbol,
            INamedTypeSymbol assetPrefixAttributeTypeSymbol)
        {
            var typeSymbol = (INamedTypeSymbol)context.Symbol;
            if (typeSymbol.TypeKind != TypeKind.Class)
                return;

            if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentTypeSymbol))
                return;

            AttributeData assetRootAttribute = null;
            AttributeData assetPrefixAttribute = null;

            foreach (var attribute in typeSymbol.GetAttributes())
            {
                if (assetRootAttribute == null &&
                    SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, assetRootAttributeTypeSymbol))
                {
                    assetRootAttribute = attribute;
                    continue;
                }

                if (assetPrefixAttribute == null &&
                    SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, assetPrefixAttributeTypeSymbol))
                {
                    assetPrefixAttribute = attribute;
                }
            }

            if (assetRootAttribute == null || assetPrefixAttribute == null)
                return;

            var attributeSyntax =
                assetPrefixAttribute.ApplicationSyntaxReference?.GetSyntax(context.CancellationToken) as AttributeSyntax;
            var location = attributeSyntax?.GetLocation() ?? typeSymbol.Locations.FirstOrDefault();
            if (location == null)
                return;

            var diagnostic = Diagnostic.Create(Rule, location, typeSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
