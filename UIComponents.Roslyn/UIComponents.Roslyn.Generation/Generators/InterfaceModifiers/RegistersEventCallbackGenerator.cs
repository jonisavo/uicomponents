using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.InterfaceModifiers
{
    [Generator]
    public sealed class RegistersEventCallbackGenerator : InterfaceModifierGenerator<RegistersEventCallbackDescription>
    {
        protected override string GetAttributeName()
        {
            return "UIComponents.InterfaceModifiers.RegistersEventCallbackAttribute";
        }

        private string GetCallbackNameForType(INamedTypeSymbol typeSymbol)
        {
            var typeName = typeSymbol.Name;

            if (typeName.StartsWith("I"))
                typeName = typeName.Substring(1);

            return typeName;
        }

        protected override void GetModifierDescriptions(InterfaceAttributeInfo attributeInfo, List<RegistersEventCallbackDescription> output)
        {
            foreach (var attributeArgs in attributeInfo.Attributes.Values)
            {
                INamedTypeSymbol eventType = null;

                if (attributeArgs.TryGetValue("constructor_0", out var typeArg))
                    if (typeArg.Value is INamedTypeSymbol callbackType)
                        eventType = callbackType;

                if (eventType == null)
                    continue;

                string methodName = GetCallbackNameForType(attributeInfo.InterfaceType);

                if (attributeArgs.TryGetValue("constructor_1", out var methodNameArg))
                    if (methodNameArg.Value is string methodNameString)
                        methodName = methodNameString;

                output.Add(new RegistersEventCallbackDescription(eventType, attributeInfo.InterfaceType, methodName));
            }
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding("protected override void UIC_RegisterEventCallbacks()")
                .AppendLineWithPadding("{");

            foreach (var description in ModifierDescriptions)
                stringBuilder
                    .AppendWithPadding("RegisterCallback<", 2)
                    .Append(context.GetTypeName(description.EventType))
                    .Append(">(")
                    .Append(description.MethodName)
                    .AppendLine(");");

            stringBuilder.AppendLineWithPadding("}").AppendLine();

            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding("protected override void UIC_UnregisterEventCallbacks()")
                .AppendLineWithPadding("{");

            foreach (var description in ModifierDescriptions)
                stringBuilder
                    .AppendWithPadding("UnregisterCallback<", 2)
                    .Append(context.GetTypeName(description.EventType))
                    .Append(">(")
                    .Append(description.MethodName)
                    .AppendLine(");");

            stringBuilder.AppendLineWithPadding("}");
        }
    }
}

