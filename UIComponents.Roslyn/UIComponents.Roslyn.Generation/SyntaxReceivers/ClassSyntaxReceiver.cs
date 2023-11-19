using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    public class ClassSyntaxReceiver : ISyntaxReceiverWithTypes
    {
        private readonly List<ClassDeclarationSyntax> _classes =
            new List<ClassDeclarationSyntax>();

        public virtual void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                _classes.Add(classDeclarationSyntax);
        }

        public IReadOnlyList<TypeDeclarationSyntax> GetTypes() => _classes;
    }
}
