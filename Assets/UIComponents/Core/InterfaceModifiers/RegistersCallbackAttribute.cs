using System;
using System.Diagnostics;
using UnityEngine.TestTools;

namespace UIComponents.InterfaceModifiers
{
    /// <summary>
    /// When applied on an interface, its inheritors will have code generated
    /// for register an event callback.
    /// <para />
    /// The name of the callback method is determined from the interface name.
    /// E.g. IOnClick -> OnClick.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    [ExcludeFromCoverage]
    [Conditional("UICOMPONENTS_INCLUDE_ATTRIBUTES")]
    public class RegistersCallbackAttribute : Attribute
    {
        public readonly Type EventType;

        public RegistersCallbackAttribute(Type eventType)
        {
            EventType = eventType;
        }
    }
}