using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;

namespace UIComponents.Roslyn.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InvalidReadonlyCodeFixProvider)), Shared]
    public sealed class InvalidReadonlyCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(InvalidReadonlyMemberAnalyzer.DiagnosticId); }
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

            var fieldDeclaration = root.FindToken(diagnosticSpan.Start).Parent
                .AncestorsAndSelf()
                .OfType<FieldDeclarationSyntax>()
                .First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.InvalidReadonlyFix_Title,
                    createChangedDocument: cancellationToken => RemoveReadonly(context.Document, fieldDeclaration, cancellationToken),
                    equivalenceKey: nameof(CodeFixResources.InvalidReadonlyFix_Title)),
                diagnostic);
        }

        private async Task<Document> RemoveReadonly(Document document, FieldDeclarationSyntax fieldDeclaration, CancellationToken cancellationToken)
        {
            var typeDeclaration = fieldDeclaration
                .Ancestors()
                .OfType<BaseTypeDeclarationSyntax>()
                .First();

            var editor = new SyntaxEditor(typeDeclaration, document.Project.Solution.Workspace);

            var newModifiers = fieldDeclaration.Modifiers
                .Where(modifier => modifier.Kind() != SyntaxKind.ReadOnlyKeyword);
            var newFieldDeclaration = fieldDeclaration.WithModifiers(new SyntaxTokenList(newModifiers));
            editor.ReplaceNode(fieldDeclaration, newFieldDeclaration);

            var newDeclaration = editor.GetChangedRoot();

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(typeDeclaration, newDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
    }
}
