using Microsoft.CodeAnalysis;

namespace UIComponents.Roslyn.Generation.Generators.InterfaceModifiers
{
    public readonly struct RegistersEventCallbackDescription
    {
        public readonly INamedTypeSymbol EventType;
        public readonly INamedTypeSymbol InterfaceType;
        public readonly string MethodName;

        public RegistersEventCallbackDescription(INamedTypeSymbol eventType, INamedTypeSymbol interfaceType, string methodName)
        {
            EventType = eventType;
            InterfaceType = interfaceType;
            MethodName = methodName;
        }
    }
}

