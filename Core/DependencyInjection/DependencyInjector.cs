using System;
using System.Collections.Generic;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// Class responsible for providing dependencies to a consumer.
    /// </summary>
    public sealed class DependencyInjector
    {
        /// <summary>
        /// Whether the injector has been initialized with a consumer.
        /// </summary>
        public bool HasConsumer { get; private set; }
        
        private readonly Dictionary<Type, Type> _singletonTypeDictionary
            = new Dictionary<Type, Type>();
        private readonly IReadOnlyDictionary<Type, object> _singletonInstanceDictionary;
        private readonly Dictionary<Type, object> _singletonOverrideDictionary
            = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> _transientInstanceDictionary
            = new Dictionary<Type, object>();

        public DependencyInjector(DiContext context)
        {
            _singletonInstanceDictionary = context.GetSingletonInstances();
        }

        public DependencyInjector(IDependencyConsumer consumer, DiContext context) : this(context)
        {
            SetConsumer(consumer);
        }

        /// <summary>
        /// Initializes the injector using the given consumer.
        /// </summary>
        /// <param name="consumer">Dependency consumer to use for the injector</param>
        public void SetConsumer(IDependencyConsumer consumer)
        {
            if (HasConsumer)
                _transientInstanceDictionary.Clear();

            foreach (var dependency in consumer.GetDependencies())
            {
                var dependencyType = dependency.GetDependencyType();

                if (dependency.GetScope() == Scope.Singleton)
                {
                    var implementationType = dependency.GetImplementationType();

                    _singletonTypeDictionary[dependencyType] = implementationType;

                    continue;
                }

                if (_transientInstanceDictionary.ContainsKey(dependencyType))
                    continue;

                _transientInstanceDictionary[dependencyType] = dependency.CreateInstance();
            }

            HasConsumer = true;
        }

        /// <summary>
        /// Returns a dependency. Throws a <see cref="MissingProviderException"/>
        /// if the dependency can not be provided.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="MissingProviderException">
        /// Thrown if the dependency can not be provided
        /// </exception>
        /// <returns>Dependency instance</returns>
        public T Provide<T>() where T : class
        {
            var couldProvide = TryProvide<T>(out var instance);
            
            if (!couldProvide)
                throw new MissingProviderException(typeof(T));

            return instance;
        }

        /// <summary>
        /// Attempts to fetch a dependency. Returns whether
        /// the dependency could be fetched.
        /// </summary>
        /// <param name="instance">Dependency instance</param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <returns>Whether the dependency could be fetched</returns>
        public bool TryProvide<T>(out T instance) where T : class
        {
            var dependencyType = typeof(T);

            instance = null;

            if (_singletonTypeDictionary.ContainsKey(dependencyType))
            {
                var implementationType = _singletonTypeDictionary[dependencyType];

                if (_singletonInstanceDictionary.TryGetValue(implementationType, out object instanceObj))
                    instance = instanceObj as T;
            }

            if (_singletonOverrideDictionary.ContainsKey(dependencyType))
                instance = _singletonOverrideDictionary[dependencyType] as T;

            if (_transientInstanceDictionary.ContainsKey(dependencyType))
                instance = _transientInstanceDictionary[dependencyType] as T;

            return instance != null;
        }

        /// <summary>
        /// Sets the instance of a transient dependency.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <param name="instance">New instance</param>
        public void SetTransientInstance<TDependency>(TDependency instance) where TDependency : class
        {
            _transientInstanceDictionary[typeof(TDependency)] = instance;
        }

        /// <summary>
        /// Overrides a singleton instance.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <param name="instance">New singleton instance</param>
        public void SetSingletonOverride<TDependency>(TDependency instance) where TDependency : class
        {
            _singletonOverrideDictionary[typeof(TDependency)] = instance;
        }
    }
}