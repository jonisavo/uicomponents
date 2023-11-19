using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            
            context.RegisterCompilationStartAction((startContext) =>
            {
                var uiComponentTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
                var stylesheetAttributeTypeSymbol =
                    startContext.Compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");
                
                if (uiComponentTypeSymbol != null && stylesheetAttributeTypeSymbol != null)
                    startContext.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
            });
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var typeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            var uiComponentTypeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
            var stylesheetAttributeTypeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");

            if (!RoslynUtilities.HasBaseType(typeSymbol, uiComponentTypeSymbol))
                return;

            var attributeLists = classDeclaration.AttributeLists;

            if (attributeLists.Count == 0)
                return;

            var stylesheets = new Dictionary<AttributeSyntax, string>();

            foreach (var attributeList in attributeLists.Reverse())
            {
                var stylesheetAttributes = attributeList.Attributes.Where((attribute) =>
                {
                    var attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol.ContainingType;

                    return SymbolEqualityComparer.Default.Equals(attributeSymbol, stylesheetAttributeTypeSymbol);
                });

                foreach (var attribute in stylesheetAttributes.Reverse())
                {
                    var arguments = attribute.ArgumentList.Arguments;
                    var firstArgument = arguments.FirstOrDefault();

                    var fullArgumentString = firstArgument.Expression.ToString();
                    var argument = fullArgumentString.Substring(1, fullArgumentString.Length - 2);

                    if (!stylesheets.ContainsValue(argument))
                        stylesheets.Add(attribute, argument);
                }
            }

            var allAttributes = RoslynUtilities.GetAllAttributesOfType(typeSymbol).Where((attribute) =>
            {
                return SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, stylesheetAttributeTypeSymbol);
            }).ToArray();

            foreach (var stylesheet in stylesheets.Keys)
            {
                var stylesheetPath = stylesheets[stylesheet];

                var count = allAttributes.Count((attribute) =>
                {
                    return attribute.ConstructorArguments.Any((argument) =>
                    {
                        return argument.Value.ToString() == stylesheetPath;
                    });
                });

                if (count > 1)
                {
                    var diagnostic = Diagnostic.Create(Rule, stylesheet.GetLocation(), stylesheets[stylesheet], typeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
