using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    public enum Scope
    {
        Singleton,
        Transient
    }

	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
	public class DependencyAttribute : Attribute
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

