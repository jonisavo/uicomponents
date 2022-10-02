using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    internal interface ISyntaxReceiverWithClasses : ISyntaxReceiver
    {
        IReadOnlyList<ClassDeclarationSyntax> GetClasses();
    }
}
