using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UIComponentInternalMethodAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UIC002";

        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.UIC002_Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.UIC002_MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.UIC002_Description), Resources.ResourceManager, typeof(Resources));
        private static readonly string Category = "Core";

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

            context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
        }

        private static void AnalyzeInvocation(OperationAnalysisContext context)
        {
            var operation = (IInvocationOperation)context.Operation;

            var method = operation.TargetMethod;

            if (!method.Name.StartsWith("UIC_"))
                return;

            var uiComponentTypeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");

            if (uiComponentTypeSymbol == null)
                return;

            if (IsCallerUIComponent(operation, uiComponentTypeSymbol))
                return;

            var containingType = method.ContainingType;

            if (!SymbolEqualityComparer.Default.Equals(containingType, uiComponentTypeSymbol))
                return;

            var syntax = operation.Syntax;

            var diagnostic = Diagnostic.Create(Rule, syntax.GetLocation(), method.Name);

            context.ReportDiagnostic(diagnostic);
        }

        private static bool IsCallerUIComponent(IInvocationOperation operation, ITypeSymbol uiComponentTypeSymbol)
        {
            var baseType = RoslynUtilities.GetBaseTypeSyntax(operation.Syntax);

            var baseTypeSymbol = operation.SemanticModel.GetDeclaredSymbol(baseType) as INamedTypeSymbol;

            return SymbolEqualityComparer.Default.Equals(baseTypeSymbol, uiComponentTypeSymbol);
        }
    }
}
