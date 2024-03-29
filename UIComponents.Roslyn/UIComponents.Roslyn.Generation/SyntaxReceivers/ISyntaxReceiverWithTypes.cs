﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.SyntaxReceivers
{
    public interface ISyntaxReceiverWithTypes : ISyntaxReceiver
    {
        IReadOnlyList<TypeDeclarationSyntax> GetTypes();
    }
}
