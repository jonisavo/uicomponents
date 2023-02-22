using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace UIComponents.Roslyn.Common.Readers
{
    public enum AttributeReadOrder
    {
        CurrentFirst,
        BaseFirst
    }

    public sealed class ClassAttributeArgumentReader : AttributeArgumentReader
    {
        private AttributeReadOrder _readOrder =
            AttributeReadOrder.CurrentFirst;

        public ClassAttributeArgumentReader(INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) : base(attributeSymbol, semanticModel) {}

        public void SetReadOrder(AttributeReadOrder mode)
        {
            _readOrder = mode;
        }

        public override void Read(SyntaxNode syntaxNode, Dictionary<AttributeData, Dictionary<string, TypedConstant>> output)
        {
            var typeSymbol = SemanticModel.GetDeclaredSymbol(syntaxNode) as INamedTypeSymbol;

            ReadWithSymbol(typeSymbol, output);
        }

        public void ReadWithSymbol(INamedTypeSymbol typeSymbol, Dictionary<AttributeData, Dictionary<string, TypedConstant>> output)
        {
            if (_readOrder == AttributeReadOrder.CurrentFirst)
                GetArgumentsCurrentFirst(typeSymbol, output);
            else if (_readOrder == AttributeReadOrder.BaseFirst)
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
