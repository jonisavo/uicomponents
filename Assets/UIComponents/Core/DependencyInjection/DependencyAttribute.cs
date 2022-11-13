using System;
using System.Diagnostics;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// Used to define a dependency for UIComponents.
    /// </summary>
    /// <seealso cref="UIComponent"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly, AllowMultiple = true)]
    [ExcludeFromCoverage]
    [Conditional("UICOMPONENTS_INCLUDE_ATTRIBUTES")]
    public sealed class DependencyAttribute : Attribute
    {
        public readonly Type DependencyType;

        public readonly Type ImplementationType;
        
        public readonly Scope Scope;

        public DependencyAttribute(Type dependency, Type provide, Scope scope = Scope.Singleton)
        {
            DependencyType = dependency;
            ImplementationType = provide;
            Scope = scope;
        }
    }
}
