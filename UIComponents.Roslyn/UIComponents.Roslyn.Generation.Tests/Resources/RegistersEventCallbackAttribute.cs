using System.Diagnostics.CodeAnalysis;

namespace UIComponents.InterfaceModifiers
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class RegistersEventCallbackAttribute : Attribute
    {
        public readonly Type EventType;
        public readonly string? MethodName;

        public RegistersEventCallbackAttribute(Type eventType, string? methodName = null)
        {
            EventType = eventType;
            MethodName = methodName;
        }
    }
}

