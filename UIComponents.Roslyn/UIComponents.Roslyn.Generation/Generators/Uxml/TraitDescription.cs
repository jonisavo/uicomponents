using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    internal readonly struct TraitDescription
    {
        public readonly string ClassMemberName;
        public readonly string UxmlName;
        public readonly string TraitMemberName;
        public readonly string Type;
        public readonly string DefaultValue;

        private const string NameArgumentName = "Name";
        private const string DefaultValueArgumentName = "DefaultValue";

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

        private static (string, string) GetTraitUxmlNameAndDefaultValue(
            Dictionary<string, TypedConstant> traitArguments,
            ITypeSymbol typeSymbol,
            string memberName)
        {
            string uxmlName = null;
            string defaultValue = null;

            if (traitArguments.ContainsKey(NameArgumentName))
                uxmlName = traitArguments[NameArgumentName].Value?.ToString();

            if (string.IsNullOrEmpty(uxmlName))
                uxmlName = memberName.ToLower();

            if (traitArguments.ContainsKey(DefaultValueArgumentName))
            {
                var defaultValueString = traitArguments[DefaultValueArgumentName].Value?.ToString();

                var defaultValueObject = traitArguments[DefaultValueArgumentName].Value;

                if (!string.IsNullOrEmpty(defaultValueString) && defaultValueObject is string)
                    defaultValueString = StringUtilities.AddQuotesToString(defaultValueString);
                if (!string.IsNullOrEmpty(defaultValueString) && defaultValueObject is bool)
                    defaultValueString = defaultValueString.ToLower();
                if (!string.IsNullOrEmpty(defaultValueString) && typeSymbol.TypeKind == TypeKind.Enum)
                    defaultValueString = $"({typeSymbol.ToDisplayString()}) {defaultValueString}";
                defaultValue = defaultValueString;
            }

            return (uxmlName, defaultValue);
        }

        public static TraitDescription CreateFromFieldSymbol(IFieldSymbol typeSymbol, Dictionary<string, TypedConstant> arguments)
        {
            var (uxmlName, defaultValue) = GetTraitUxmlNameAndDefaultValue(arguments, typeSymbol.Type, typeSymbol.Name);

            return new TraitDescription(typeSymbol.Name, uxmlName, (INamedTypeSymbol)typeSymbol.Type, defaultValue);
        }

        public static TraitDescription CreateFromPropertySymbol(IPropertySymbol propertySymbol, Dictionary<string, TypedConstant> arguments)
        {
            var (uxmlName, defaultValue) = GetTraitUxmlNameAndDefaultValue(arguments, propertySymbol.Type, propertySymbol.Name);

            return new TraitDescription(propertySymbol.Name, uxmlName, (INamedTypeSymbol)propertySymbol.Type, defaultValue);
        }
    }
}