using System;
using JetBrains.Annotations;

namespace UIComponents.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    public class InjectDependencyAttribute : Attribute
    {
        public readonly Type DependencyType;

        public readonly Type ProviderType;

        public InjectDependencyAttribute(Type dependency, Type provider)
        {
            DependencyType = dependency;
            ProviderType = provider;
        }
    }
}