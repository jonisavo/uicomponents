using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class EmptyUxmlNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC102";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC102_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC102_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC102_Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Layout";

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

            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var typeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            var uiComponentTypeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
            var uxmlNameAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UxmlNameAttribute");

            if (uiComponentTypeSymbol == null || uxmlNameAttributeSymbol == null)
                return;

            if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentTypeSymbol))
                return;

            var attributeLists = classDeclaration.AttributeLists;

            foreach (var attributeList in attributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    var symbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol.ContainingType;

                    if (!SymbolEqualityComparer.Default.Equals(symbol, uxmlNameAttributeSymbol))
                        continue;

                    var argument = attribute.ArgumentList.Arguments.FirstOrDefault();
                    var fullArgumentString = argument.Expression.ToString();
                    
                    if (fullArgumentString == "\"\"" || fullArgumentString == "null")
                    {                         
                        var diagnostic = Diagnostic.Create(Rule, argument.GetLocation());        
                        context.ReportDiagnostic(diagnostic);             
                    }
                }
            }
        }
    }
}
