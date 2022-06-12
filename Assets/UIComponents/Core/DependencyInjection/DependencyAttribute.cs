using System;

namespace UIComponents
{
    /// <summary>
    /// Used to define a dependency for a UIComponent.
    /// </summary>
    /// <seealso cref="UIComponent"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyAttribute : Attribute
    {
        public readonly Type DependencyType;

        public readonly Type ProvideType;

        public DependencyAttribute(Type dependency, Type provide)
        {
            DependencyType = dependency;
            ProvideType = provide;
        }
    }
}