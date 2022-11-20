using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
	public class TypeSyntaxReceiver : ISyntaxReceiverWithTypes
	{
        public readonly List<TypeDeclarationSyntax> Types =
            new List<TypeDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is TypeDeclarationSyntax classDeclarationSyntax))
                return;

            Types.Add(classDeclarationSyntax);
        }

        public IReadOnlyList<TypeDeclarationSyntax> GetTypes() => Types;
    }
}

