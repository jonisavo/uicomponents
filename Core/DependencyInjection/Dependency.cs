using System;

namespace UIComponents.DependencyInjection
{
    public static class Dependency
    {
        public static IDependency SingletonFor<T, I>() where T : class where I : T, new()
        {
            return new Dependency<T, I>(Scope.Singleton, () => new I());
        }

        public static IDependency TransientFor<T, I>() where T: class where I : T, new()
        {
            return new Dependency<T, I>(Scope.Transient, () => new I());
        }
    }

    public readonly struct Dependency<T, I> : IDependency where T : class where I : T
    {
        private readonly Type _dependencyType;
        private readonly Type _implementationType;
        private readonly Scope _scope;
        private readonly Func<I> _factory;

        public Dependency(Scope scope, Func<I> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _dependencyType = typeof(T);
            _implementationType = typeof(I);
            _scope = scope;
            _factory = factory;
        }

        public Type GetDependencyType()
        {
            return _dependencyType;
        }

        public Type GetImplementationType()
        {
            return _implementationType;
        }

        public Scope GetScope()
        {
            return _scope;
        }

        public object CreateInstance()
        {
            return _factory();
        }
    }
}