using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class EmptyUxmlTraitNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC103";
        private const string NameArgumentName = "Name";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC103_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC103_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC103_Description), Resources.ResourceManager, typeof(Resources));
        private static readonly string Category = "Layout";

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

            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.FieldDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.PropertyDeclaration);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var memberDeclaration = (MemberDeclarationSyntax)context.Node;

            var parentNode = memberDeclaration.Parent;

            if (!(parentNode is ClassDeclarationSyntax parentClass))
                return;

            var classTypeSymbol = context.SemanticModel.GetDeclaredSymbol(parentClass);

            var uiComponentTypeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
            var uxmlTraitAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UxmlTraitAttribute");

            if (uiComponentTypeSymbol == null || uxmlTraitAttributeSymbol == null)
                return;

            if (!RoslynUtilities.HasBaseType(classTypeSymbol, uiComponentTypeSymbol))
                return;

            var attributeLists = memberDeclaration.AttributeLists;

            foreach (var attributeList in attributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.ArgumentList == null)
                        continue;

                    var symbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;

                    if (symbol == null)
                        continue;

                    var typeSymbol = symbol.ContainingType;

                    if (typeSymbol == null)
                        continue;

                    if (!SymbolEqualityComparer.Default.Equals(typeSymbol, uxmlTraitAttributeSymbol))
                        continue;

                    var argument = attribute.ArgumentList.Arguments.Where(
                        (arg) => {
                            if (arg.NameEquals == null)
                                return false;

                            return arg.NameEquals.Name.Identifier.Text == NameArgumentName;
                        }
                    ).FirstOrDefault();

                    if (argument == null)
                        continue;

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
