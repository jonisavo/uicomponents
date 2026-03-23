using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class EmptyAssetPathAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC104";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC104_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC104_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC104_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Layout";

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error,
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
                var layoutAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.LayoutAttribute");
                var stylesheetAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");
                var sharedStylesheetAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.SharedStylesheetAttribute");
                var assetRootAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.AssetRootAttribute");
                var assetPrefixAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.AssetPrefixAttribute");

                if (uiComponentTypeSymbol == null ||
                    layoutAttributeSymbol == null ||
                    stylesheetAttributeSymbol == null ||
                    sharedStylesheetAttributeSymbol == null ||
                    assetRootAttributeSymbol == null)
                    return;

                startContext.RegisterSyntaxNodeAction(syntaxContext =>
                    AnalyzeSyntaxNode(
                        syntaxContext,
                        uiComponentTypeSymbol,
                        layoutAttributeSymbol,
                        stylesheetAttributeSymbol,
                        sharedStylesheetAttributeSymbol,
                        assetRootAttributeSymbol,
                        assetPrefixAttributeSymbol),
                    SyntaxKind.ClassDeclaration);
            });
        }

        private static void AnalyzeSyntaxNode(
            SyntaxNodeAnalysisContext context,
            INamedTypeSymbol uiComponentTypeSymbol,
            INamedTypeSymbol layoutAttributeSymbol,
            INamedTypeSymbol stylesheetAttributeSymbol,
            INamedTypeSymbol sharedStylesheetAttributeSymbol,
            INamedTypeSymbol assetRootAttributeSymbol,
            INamedTypeSymbol assetPrefixAttributeSymbol)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            var typeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentTypeSymbol))
                return;

            foreach (var attributeList in classDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.ArgumentList == null || attribute.ArgumentList.Arguments.Count == 0)
                        continue;

                    var symbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;
                    var attributeTypeSymbol = symbol?.ContainingType;

                    if (attributeTypeSymbol == null)
                        continue;

                    if (!IsTrackedAttribute(
                            attributeTypeSymbol,
                            layoutAttributeSymbol,
                            stylesheetAttributeSymbol,
                            sharedStylesheetAttributeSymbol,
                            assetRootAttributeSymbol,
                            assetPrefixAttributeSymbol))
                        continue;

                    var argument = attribute.ArgumentList.Arguments[0];
                    var constantValue = context.SemanticModel.GetConstantValue(argument.Expression);

                    if (!constantValue.HasValue)
                        continue;

                    var value = constantValue.Value as string;
                    if (constantValue.Value != null && value == null)
                        continue;

                    if (!string.IsNullOrWhiteSpace(value))
                        continue;

                    var diagnostic = Diagnostic.Create(
                        Rule,
                        argument.GetLocation(),
                        attributeTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private static bool IsTrackedAttribute(
            INamedTypeSymbol attributeTypeSymbol,
            INamedTypeSymbol layoutAttributeSymbol,
            INamedTypeSymbol stylesheetAttributeSymbol,
            INamedTypeSymbol sharedStylesheetAttributeSymbol,
            INamedTypeSymbol assetRootAttributeSymbol,
            INamedTypeSymbol assetPrefixAttributeSymbol)
        {
            return SymbolEqualityComparer.Default.Equals(attributeTypeSymbol, layoutAttributeSymbol) ||
                SymbolEqualityComparer.Default.Equals(attributeTypeSymbol, stylesheetAttributeSymbol) ||
                SymbolEqualityComparer.Default.Equals(attributeTypeSymbol, sharedStylesheetAttributeSymbol) ||
                SymbolEqualityComparer.Default.Equals(attributeTypeSymbol, assetRootAttributeSymbol) ||
                (assetPrefixAttributeSymbol != null &&
                 SymbolEqualityComparer.Default.Equals(attributeTypeSymbol, assetPrefixAttributeSymbol));
        }
    }
}
