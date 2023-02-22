using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    public class ClassSyntaxReceiver : ISyntaxReceiverWithTypes
    {
        public readonly List<ClassDeclarationSyntax> Classes =
            new List<ClassDeclarationSyntax>();

        public virtual void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                Classes.Add(classDeclarationSyntax);
        }

        public IReadOnlyList<TypeDeclarationSyntax> GetTypes() => Classes;
    }
}
