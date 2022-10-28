using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Readers;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.InterfaceModifiers
{
    internal class RegistersCallbackGenerator : InterfaceModifierGenerator<RegistersCallbackDescription>
    {
        protected override string GetAttributeName()
        {
            return "UIComponents.InterfaceModifiers.RegistersCallbackAttribute";
        }

        protected override void GetModifierDescriptions(InterfaceAttributeInfo attributeInfo, List<RegistersCallbackDescription> output)
        {
            foreach (var attributeArgs in attributeInfo.Attributes.Values)
            {
                if (attributeArgs.TryGetValue("constructor_0", out var typeArg))
                    if (typeArg.Value is INamedTypeSymbol callbackType)
                        output.Add(new RegistersCallbackDescription(callbackType, attributeInfo.InterfaceType));
            }
        }

        private string GetCallbackNameForType(INamedTypeSymbol typeSymbol)
        {
            var typeName = typeSymbol.Name;

            if (typeName.StartsWith("I"))
                typeName = typeName.Substring(1);

            return typeName;
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding("protected override void UIC_RegisterCallbacks()")
                .AppendLineWithPadding("{");

            foreach (var description in ModifierDescriptions)
                stringBuilder
                    .AppendWithPadding("RegisterCallback<", 2)
                    .Append(context.GetTypeName(description.EventType))
                    .Append(">(")
                    .Append(GetCallbackNameForType(description.InterfaceType))
                    .AppendLine(");");

            stringBuilder.AppendLineWithPadding("}").AppendLine();

            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding("protected override void UIC_DeregisterCallbacks()")
                .AppendLineWithPadding("{");

            foreach (var description in ModifierDescriptions)
                stringBuilder
                    .AppendWithPadding("DeregisterCallback<", 2)
                    .Append(context.GetTypeName(description.EventType))
                    .Append(">(")
                    .Append(GetCallbackNameForType(description.InterfaceType))
                    .AppendLine(");");

            stringBuilder.AppendLineWithPadding("}");
        }
    }
}

