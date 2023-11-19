using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
	public class TypeSyntaxReceiver : ISyntaxReceiverWithTypes
	{
        private readonly List<TypeDeclarationSyntax> _types =
            new List<TypeDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
                _types.Add(typeDeclarationSyntax);
        }

        public IReadOnlyList<TypeDeclarationSyntax> GetTypes() => _types;
    }
}

