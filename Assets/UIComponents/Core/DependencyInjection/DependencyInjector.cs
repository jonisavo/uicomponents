using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UIComponents
{
    /// <summary>
    /// The class responsible for providing UIComponents with their
    /// dependencies.
    /// </summary>
    /// <seealso cref="UIComponent"/>
    /// <seealso cref="DependencyAttribute"/>
    public class DependencyInjector
    {
        /// <summary>
        /// A container used with the static DependencyInjector methods.
        /// </summary>
        internal static readonly DiContainer Container = new DiContainer();

        /// <summary>
        /// Contains the dependencies the injector provides to its consumer.
        /// </summary>
        private readonly Dictionary<Type, Dependency> _dependencyDictionary
            = new Dictionary<Type, Dependency>();

        /// <summary>
        /// Switches the dependency of a consumer.
        /// </summary>
        /// <remarks>
        /// Can be used in unit tests to switch to
        /// a mocked dependency.
        /// </remarks>
        /// <param name="provider">
        /// The new instance used for the dependency
        /// </param>
        /// <typeparam name="TConsumer">Consumer type</typeparam>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        public static void SetDependency<TConsumer, TDependency>(TDependency provider)
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));
            
            injector.SetDependency(provider);
        }

        /// <summary>
        /// Clears the dependency of a consumer.
        /// </summary>
        /// <remarks>
        /// Can be used in unit tests to clear
        /// a dependency between tests.
        /// </remarks>
        /// <typeparam name="TConsumer">Consumer type</typeparam>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        public static void ClearDependency<TConsumer, TDependency>()
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));
            
            injector.ClearDependency<TDependency>();
        }
        
        /// <summary>
        /// Restores the default dependency, which is the one
        /// set by <see cref="DependencyAttribute"/>.
        /// </summary>
        /// <remarks>
        /// Can be used in unit tests to clear
        /// restore dependency between or after tests.
        /// </remarks>
        /// <typeparam name="TConsumer">Consumer type</typeparam>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no default dependency type exists
        /// </exception>
        public static void RestoreDefaultDependency<TConsumer, TDependency>()
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));
            
            injector.RestoreDefaultDependency<TDependency>();
        }

        /// <summary>
        /// Returns the injector of the given consumer type.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        /// <returns>Injector of the consumer type</returns>
        public static DependencyInjector GetInjector(Type consumerType)
        {
            return Container.GetInjector(consumerType);
        }
        
        /// <summary>
        /// Removes the injector of the given consumer type.
        /// Used primarily for testing.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        public static void RemoveInjector(Type consumerType)
        {
            Container.RemoveInjector(consumerType);
        }

        /// <summary>
        /// Constructs a new injector with no dependencies configured.
        /// </summary>
        public DependencyInjector() {}

        /// <summary>
        /// Constructs a new injector with dependencies configured
        /// according to the given DependencyAttributes.
        /// </summary>
        /// <param name="dependencyAttributes">Dependency attributes</param>
        public DependencyInjector(IEnumerable<DependencyAttribute> dependencyAttributes)
        {
            foreach (var dependencyAttribute in dependencyAttributes)
            {
                var dependencyType = dependencyAttribute.DependencyType;
                
                if (_dependencyDictionary.ContainsKey(dependencyType))
                    continue;

                var providerType = dependencyAttribute.ProvideType;
                var scope = dependencyAttribute.Scope;

                var dependency = Container.CreateDependency(dependencyType, providerType, scope);

                _dependencyDictionary.Add(dependencyType, dependency);
            }
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
            
            var dependency = Container.CreateDependency(dependencyType, instance, scope);
            
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
        /// Restores the default dependency. If a singleton instance
        /// exists, it is restored. Otherwise, a new instance
        /// of the dependency is created.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no configured dependency exists
        /// </exception>
        public void RestoreDefaultDependency<T>() where T : class
        {
            var dependencyType = typeof(T);
            
            if (!_dependencyDictionary.ContainsKey(dependencyType))
                throw new InvalidOperationException($"No dependency configured for {dependencyType}");

            var dependency = _dependencyDictionary[dependencyType];
            var initialProviderType = dependency.InitialProviderType;
            
            object instance = null;

            if (dependency.Scope == Scope.Singleton)
                Container.TryGetSingletonInstance(initialProviderType, out instance);

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