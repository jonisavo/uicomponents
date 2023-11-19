using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class InvalidReadonlyMemberAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC003";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC003_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC003_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC003_Description), Resources.ResourceManager, typeof(Resources));
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
                var provideAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.ProvideAttribute");
                var queryAttributeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.QueryAttribute");
                
                if (provideAttributeSymbol != null && queryAttributeSymbol != null)
                    startContext.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.FieldDeclaration);
            });
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var fieldDeclaration = (FieldDeclarationSyntax)context.Node;

            if (!fieldDeclaration.Modifiers.Any(SyntaxKind.ReadOnlyKeyword))
                return;

            var provideAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.ProvideAttribute");
            var queryAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.QueryAttribute");

            var attributeLists = fieldDeclaration.AttributeLists;

            foreach (var attributeList in attributeLists)
            {
                var found = false;

                foreach (var attribute in attributeList.Attributes)
                {
                    var symbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;

                    if (symbol == null)
                        continue;

                    var typeSymbol = symbol.ContainingType;

                    if (typeSymbol == null)
                        continue;

                    if (!SymbolEqualityComparer.Default.Equals(typeSymbol, provideAttributeSymbol)
                        && !SymbolEqualityComparer.Default.Equals(typeSymbol, queryAttributeSymbol))
                        continue;

                    var diagnostic = Diagnostic.Create(Rule, fieldDeclaration.GetLocation(), typeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                    found = true;

                    break;
                }

                if (found)
                    break;
            }
        }
    }
}
