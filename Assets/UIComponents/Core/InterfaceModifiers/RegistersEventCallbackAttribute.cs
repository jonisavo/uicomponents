using System;
using UnityEngine.TestTools;

namespace UIComponents.InterfaceModifiers
{
    /// <summary>
    /// When applied to an interface, its inheritors will have code generated for
    /// registering an unregistering callbacks for the given Event type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    [ExcludeFromCoverage]
    public sealed class RegistersEventCallbackAttribute : Attribute
    {
        public readonly Type EventType;

        public readonly string MethodName;
        
        public RegistersEventCallbackAttribute(Type eventType, string methodName = null)
        {
            EventType = eventType;
            MethodName = methodName;
        }
    }
}
