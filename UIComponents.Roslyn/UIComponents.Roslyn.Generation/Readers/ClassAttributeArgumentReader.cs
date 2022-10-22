using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Generation.Readers
{
    internal enum ClassAttributeArgumentReaderMode
    {
        CurrentFirst,
        BaseFirst
    }

    internal class ClassAttributeArgumentReader : AttributeArgumentReader
    {
        private ClassAttributeArgumentReaderMode _mode =
            ClassAttributeArgumentReaderMode.CurrentFirst;

        public ClassAttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public void SetMode(ClassAttributeArgumentReaderMode mode)
        {
            _mode = mode;
        }

        public override void Read(SyntaxNode syntaxNode, Dictionary<AttributeData, Dictionary<string, TypedConstant>> output)
        {
            var typeSymbol = SemanticModel.GetDeclaredSymbol(syntaxNode) as INamedTypeSymbol;

            if (_mode == ClassAttributeArgumentReaderMode.CurrentFirst)
                GetArgumentsCurrentFirst(typeSymbol, output);
            else if (_mode == ClassAttributeArgumentReaderMode.BaseFirst)
                GetArgumentBaseFirst(typeSymbol, output);
        }

        private void GetArgumentsCurrentFirst(INamedTypeSymbol typeSymbol, Dictionary<AttributeData, Dictionary<string, TypedConstant>> output)
        {
            while (typeSymbol != null)
            {
                GetArgumentsOfSymbol(typeSymbol, output);
                typeSymbol = typeSymbol.BaseType;
            }
        }

        private void GetArgumentBaseFirst(INamedTypeSymbol typeSymbol, Dictionary<AttributeData, Dictionary<string, TypedConstant>> output)
        {
            var baseTypeList = new List<INamedTypeSymbol>();

            while (typeSymbol != null)
            {
                baseTypeList.Insert(0, typeSymbol);
                typeSymbol = typeSymbol.BaseType;
            }

            foreach (var type in baseTypeList)
                GetArgumentsOfSymbol(type, output);
        }
    }
}
