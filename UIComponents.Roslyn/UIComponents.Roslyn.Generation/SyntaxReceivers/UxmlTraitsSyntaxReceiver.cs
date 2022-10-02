using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    internal sealed class UxmlTraitsSyntaxReceiver : ISyntaxReceiverWithClasses
    {
        public readonly List<ClassDeclarationSyntax> Classes =
            new List<ClassDeclarationSyntax>();
        public readonly List<MemberDeclarationSyntax> FieldsAndProperties =
            new List<MemberDeclarationSyntax>();

        private bool NodeIsFieldOrProperty(SyntaxNode syntaxNode)
        {
            return syntaxNode is FieldDeclarationSyntax || syntaxNode is PropertyDeclarationSyntax;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                Classes.Add(classDeclarationSyntax);

            if (!NodeIsFieldOrProperty(syntaxNode))
                return;

            var memberDeclarationSyntax = syntaxNode as MemberDeclarationSyntax;

            if (memberDeclarationSyntax.AttributeLists.Count > 0)
                FieldsAndProperties.Add(memberDeclarationSyntax);
        }

        public IReadOnlyList<ClassDeclarationSyntax> GetClasses() => Classes;
    }
}
