using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UIComponentPartialAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC001";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC001_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC001_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC001_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Core";

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            
            context.RegisterCompilationStartAction((startContext) =>
            {
                var uiComponentTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
                
                if (uiComponentTypeSymbol != null)
                    startContext.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            });
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax) context.Node;

            var typeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            if (typeSymbol == null || typeSymbol.IsAbstract)
                return;

            var uiComponentTypeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");

            if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentTypeSymbol))
                return;
            
            if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                var diagnostic = Diagnostic.Create(Rule, typeSymbol.Locations[0], typeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
