using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// The class responsible for providing UIComponents with their
    /// dependencies.
    /// </summary>
    /// <seealso cref="UIComponent"/>
    /// <seealso cref="DependencyAttribute"/>
    public class DependencyInjector
    {
        private readonly DiContainer _container;

        /// <summary>
        /// Contains the dependencies the injector provides to its consumer.
        /// </summary>
        private readonly Dictionary<Type, Dependency> _dependencyDictionary
            = new Dictionary<Type, Dependency>();

        /// <summary>
        /// Constructs a new injector with no dependencies configured.
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public DependencyInjector(DiContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Constructs a new injector with dependencies configured
        /// according to the given DependencyAttributes.
        /// </summary>
        /// <param name="dependencyAttributes">Dependency attributes</param>
        /// <param name="container">Dependency injection container</param>
        public DependencyInjector(IEnumerable<DependencyAttribute> dependencyAttributes, DiContainer container) : this(container)
        {
            foreach (var dependencyAttribute in dependencyAttributes)
            {
                var dependencyType = dependencyAttribute.DependencyType;

                if (_dependencyDictionary.ContainsKey(dependencyType))
                    continue;

                var providerType = dependencyAttribute.ProvideType;
                var scope = dependencyAttribute.Scope;

                var dependency = CreateDependency(dependencyType, providerType, scope);

                _dependencyDictionary.Add(dependencyType, dependency);
            }
        }
        
        private Dependency CreateSingletonDependency(Type dependencyType, Type providerType)
        {
            if (_container.TryGetSingletonOverride(dependencyType, out var overrideInstance))
                return new Dependency(dependencyType, overrideInstance, Scope.Singleton);
            
            if (_container.TryGetSingletonInstance(providerType, out var instance))
                return new Dependency(dependencyType, instance, Scope.Singleton);

            var dependency = new Dependency(dependencyType, providerType, Scope.Singleton);
            
            _container.RegisterSingletonInstance(providerType, dependency.Instance);

            return dependency;
        }

        /// <summary>
        /// Creates a new Dependency object with the given dependency and provider types.
        /// The dependency instance is created using the provider type.
        /// </summary>
        private Dependency CreateDependency(Type dependencyType, Type providerType, Scope scope)
        {
            if (scope == Scope.Singleton)
                return CreateSingletonDependency(dependencyType, providerType);
            
            return new Dependency(dependencyType, providerType, Scope.Transient);
        }
        
        /// <summary>
        /// Creates a new Dependency object with the dependency type and provided instance.
        /// </summary>
        private Dependency CreateDependency(Type dependencyType, object instance, Scope scope)
        {
            var instanceType = instance.GetType();
            
            if (scope == Scope.Singleton && !_container.ContainsSingletonInstanceOfType(instanceType))
                _container.RegisterSingletonInstance(instanceType, instance);
            
            return new Dependency(dependencyType, instance, scope);
        }

        /// <summary>
        /// Sets the instance used for a dependency.
        /// </summary>
        /// <param name="instance">New instance</param>
        /// <param name="scope">Dependency scope</param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="ArgumentNullException">
        /// Raised if the argument is null
        /// </exception>
        public void SetDependency<T>([NotNull] T instance, Scope scope = Scope.Singleton)
            where T : class
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var dependencyType = typeof(T);
            
            if (_dependencyDictionary.ContainsKey(dependencyType))
            {
                _dependencyDictionary[dependencyType].ChangeInstance(instance);
                return;
            }
            
            var dependency = CreateDependency(dependencyType, instance, scope);
            
            _dependencyDictionary.Add(dependencyType, dependency);
        }

        /// <summary>
        /// Removes the instance used for a dependency, if one is set.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        public void ClearDependency<T>() where T : class
        {
            var type = typeof(T);
            
            if (_dependencyDictionary.ContainsKey(type))
                _dependencyDictionary[type].Clear();
        }

        /// <summary>
        /// Resets the provided instance of a dependency.
        /// If a singleton instance exists, it is restored.
        /// Otherwise, a new instance of the dependency is created.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no configured dependency exists
        /// </exception>
        public void ResetProvidedInstance<T>() where T : class
        {
            var dependencyType = typeof(T);
            
            if (!_dependencyDictionary.ContainsKey(dependencyType))
                throw new InvalidOperationException($"No dependency configured for {dependencyType}");

            var dependency = _dependencyDictionary[dependencyType];
            var initialProviderType = dependency.InitialProviderType;
            
            object instance = null;

            if (dependency.Scope == Scope.Singleton)
            {
                _container.TryGetSingletonOverride(dependencyType, out instance);
                
                if (instance == null)
                    _container.TryGetSingletonInstance(initialProviderType, out instance);
            }

            if (instance == null)
                instance = Activator.CreateInstance(initialProviderType);

            _dependencyDictionary[dependencyType].ChangeInstance(instance);
        }

        private bool HasProvider(Type dependencyType)
        {
            if (!_dependencyDictionary.ContainsKey(dependencyType))
                return false;

            return _dependencyDictionary[dependencyType].Instance != null;
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
        [NotNull]
        public T Provide<T>() where T : class
        {
            var type = typeof(T);

            if (!HasProvider(type))
                throw new MissingProviderException(type);
            
            return (T) _dependencyDictionary[type].Instance;
        }
        
        /// <summary>
        /// Returns a dependency. Throws a <see cref="MissingProviderException"/>
        /// if the dependency can not be provided.
        /// </summary>
        /// <param name="type">Dependency type</param>
        /// <returns>Dependency instance</returns>
        /// <exception cref="MissingProviderException">
        /// Thrown if the dependency can not be provided
        /// </exception>
        [NotNull]
        public object Provide(Type type)
        {
            if (!HasProvider(type))
                throw new MissingProviderException(type);
            
            return _dependencyDictionary[type].Instance;
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
            instance = null;
            var type = typeof(T);

            if (!HasProvider(type))
                return false;

            instance = (T) _dependencyDictionary[type].Instance;

            return true;
        }
    }
}
