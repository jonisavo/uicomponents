using System;
using Microsoft.CodeAnalysis;

namespace UIComponents.Roslyn.Generation.Generators.InterfaceModifiers
{
    public readonly struct RegistersCallbackDescription
    {
        public readonly INamedTypeSymbol EventType;
        public readonly INamedTypeSymbol InterfaceType;

        public RegistersCallbackDescription(INamedTypeSymbol eventType, INamedTypeSymbol interfaceType)
        {
            EventType = eventType;
            InterfaceType = interfaceType;
        }
    }
}

