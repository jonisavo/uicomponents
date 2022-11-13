using System;

namespace UIComponents.DependencyInjection
{
    public readonly struct Dependency<T> : IDependency where T : class
    {
        private readonly Type _dependencyType;
        private readonly Scope _scope;
        private readonly Func<T> _factory;

        public Dependency(Scope scope, Func<T> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            _dependencyType = typeof(T);
            _scope = scope;
            _factory = factory;
        }

        public Type GetDependencyType()
        {
            return _dependencyType;
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