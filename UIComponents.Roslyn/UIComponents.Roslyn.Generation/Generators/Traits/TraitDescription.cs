using Microsoft.CodeAnalysis;
using System;

namespace UIComponents.Roslyn.Generation.Generators.Traits
{
    internal readonly struct TraitDescription
    {
        public readonly string ClassMemberName;
        public readonly string UxmlName;
        public readonly string TraitMemberName;
        public readonly string Type;
        public readonly string DefaultValue;

        public TraitDescription(string classMemberName, string uxmlName, INamedTypeSymbol typeSymbol, string defaultValue = null)
        {
            ClassMemberName = classMemberName;
            UxmlName = uxmlName;
            TraitMemberName = $"m_{ClassMemberName}";
            DefaultValue = defaultValue;

            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_String:
                    Type = "UxmlStringAttributeDescription";
                    break;
                case SpecialType.System_Single:
                    Type = "UxmlFloatAttributeDescription";
                    break;
                case SpecialType.System_Double:
                    Type = "UxmlDoubleAttributeDescription";
                    break;
                case SpecialType.System_Int32:
                    Type = "UxmlIntAttributeDescription";
                    break;
                case SpecialType.System_Int64:
                    Type = "UxmlLongAttributeDescription";
                    break;
                case SpecialType.System_Boolean:
                    Type = "UxmlBoolAttributeDescription";
                    break;
                default:
                    if (typeSymbol.TypeKind == TypeKind.Enum)
                        Type = $"UxmlEnumAttributeDescription<{typeSymbol.ToDisplayString()}>";
                    else if (typeSymbol.Name == "Color")
                        Type = "UxmlColorAttributeDescription";
                    else
                        throw new InvalidOperationException("Unknown trait field type " + typeSymbol.ToDisplayString());
                    break;
            }
        }

        public static TraitDescription CreateFromFieldSymbol(IFieldSymbol typeSymbol, string uxmlName, string defaultValue)
        {
            return new TraitDescription(typeSymbol.Name, uxmlName, (INamedTypeSymbol) typeSymbol.Type, defaultValue);
        }

        public static TraitDescription CreateFromPropertySymbol(IPropertySymbol propertySymbol, string uxmlName, string defaultValue)
        {
            return new TraitDescription(propertySymbol.Name, uxmlName, (INamedTypeSymbol)propertySymbol.Type, defaultValue);
        }
    }
}