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

namespace UIComponents.Roslyn.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InvalidAttributeCodeFixProvider)), Shared]
    public sealed class InvalidAttributeCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(UIComponentIdenticalStylesheetAnalyzer.DiagnosticId); }
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

            var attributeSyntax = root.FindToken(diagnosticSpan.Start).Parent
                .AncestorsAndSelf()
                .OfType<AttributeSyntax>()
                .First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.InvalidAttributeFix_Title,
                    createChangedDocument: cancellationToken => RemoveAttribute(context.Document, attributeSyntax, cancellationToken),
                    equivalenceKey: nameof(CodeFixResources.InvalidAttributeFix_Title)),
                diagnostic);
        }

        private async Task<Document> RemoveAttribute(Document document, AttributeSyntax attributeSyntax,  CancellationToken cancellationToken)
        {
            var attributeList = attributeSyntax.Parent as AttributeListSyntax;
            var classDeclaration = attributeList.Parent;

            var editor = new SyntaxEditor(classDeclaration, document.Project.Solution.Workspace);

            if (attributeList.Attributes.Count == 1)
                editor.RemoveNode(attributeList);
            else
                editor.RemoveNode(attributeSyntax);

            var newDeclaration = editor.GetChangedRoot();

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(classDeclaration, newDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
    }
}
