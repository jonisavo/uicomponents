using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    internal class ClassSyntaxReceiver : ISyntaxReceiverWithClasses
    {
        public readonly List<ClassDeclarationSyntax> Classes =
            new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                Classes.Add(classDeclarationSyntax);
        }

        public IReadOnlyList<ClassDeclarationSyntax> GetClasses() => Classes;
    }
}
