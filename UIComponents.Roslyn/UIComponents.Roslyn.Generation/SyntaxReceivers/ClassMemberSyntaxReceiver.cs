using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    public sealed class ClassMemberSyntaxReceiver : ClassSyntaxReceiver
    {
        public readonly List<MemberDeclarationSyntax> FieldsAndProperties =
            new List<MemberDeclarationSyntax>();

        private bool NodeIsFieldOrProperty(SyntaxNode syntaxNode)
        {
            return syntaxNode is FieldDeclarationSyntax || syntaxNode is PropertyDeclarationSyntax;
        }

        public override void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            base.OnVisitSyntaxNode(syntaxNode);

            if (!NodeIsFieldOrProperty(syntaxNode))
                return;

            var memberDeclarationSyntax = syntaxNode as MemberDeclarationSyntax;

            if (memberDeclarationSyntax.AttributeLists.Count > 0)
                FieldsAndProperties.Add(memberDeclarationSyntax);
        }
    }
}
