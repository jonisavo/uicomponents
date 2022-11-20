using JetBrains.Annotations;
using System;
using UIComponents.DependencyInjection;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// Used to define a dependency for UIComponents and other dependency consumers.
    /// </summary>
    /// <seealso cref="UIComponent"/>
    /// <seealso cref="IDependencyConsumer"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    [ExcludeFromCoverage]
    [BaseTypeRequired(typeof(IDependencyConsumer))]
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
