using System;
using System.Diagnostics.CodeAnalysis;

namespace UIComponents.InterfaceModifiers
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class RegistersCallbackAttribute : Attribute
    {
        public readonly Type EventType;

        public RegistersCallbackAttribute(Type eventType)
        {
            EventType = eventType;
        }
    }
}

