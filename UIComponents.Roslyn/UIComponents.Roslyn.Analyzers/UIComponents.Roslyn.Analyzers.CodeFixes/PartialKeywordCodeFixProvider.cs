using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UIComponents.Roslyn.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PartialKeywordCodeFixProvider)), Shared]
    public sealed class PartialKeywordCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(UIComponentPartialAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent
                .AncestorsAndSelf()
                .OfType<ClassDeclarationSyntax>()
                .First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.PartialFix_Title,
                    createChangedDocument: cancellationToken => MakePartial(context.Document, declaration, cancellationToken),
                    equivalenceKey: nameof(CodeFixResources.PartialFix_Title)),
                diagnostic);
        }

        private async Task<Document> MakePartial(Document document, ClassDeclarationSyntax classDecl, CancellationToken cancellationToken)
        {
            var editor = new SyntaxEditor(classDecl, document.Project.Solution.Workspace);
            editor.SetModifiers(classDecl, DeclarationModifiers.Partial);
            var newDeclaration = editor.GetChangedRoot();

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(classDecl, newDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
    }
}
