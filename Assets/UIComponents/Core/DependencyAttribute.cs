using System;

namespace UIComponents.Core
{
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