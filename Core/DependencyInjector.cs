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
        /// Contains all injectors created for consumers.
        /// </summary>
        internal static readonly Dictionary<Type, DependencyInjector> InjectorDictionary =
            new Dictionary<Type, DependencyInjector>();
        
        /// <summary>
        /// Contains the instantiated dependencies which are consumed
        /// by the interested parties.
        /// </summary>
        internal static readonly Dictionary<Type, object> InstantiatedInstanceDictionary
            = new Dictionary<Type, object>();

        /// <summary>
        /// Contains the dependencies the injector provides to its consumer.
        /// </summary>
        internal readonly Dictionary<Type, object> DependencyDictionary
            = new Dictionary<Type, object>();
        
        /// <summary>
        /// Contains the default provider types for each dependency of the consumer.
        /// Populated by <see cref="DependencyAttribute"/>.
        /// </summary>
        internal readonly Dictionary<Type, Type> DefaultDependencyTypeDictionary
            = new Dictionary<Type, Type>();

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
            if (InjectorDictionary.ContainsKey(consumerType))
                return InjectorDictionary[consumerType];

            return CreateInjector(consumerType);
        }
        
        /// <summary>
        /// Removes the injector of the given consumer type.
        /// Used primarily for testing.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        public static void RemoveInjector(Type consumerType)
        {
            InjectorDictionary.Remove(consumerType);
        }
        
        private static DependencyInjector CreateInjector(Type consumerType)
        {
            var injectAttributes = (DependencyAttribute[])
                consumerType.GetCustomAttributes(typeof(DependencyAttribute), true);
            
            var injector = new DependencyInjector(injectAttributes);

            InjectorDictionary.Add(consumerType, injector);

            return injector;
        }
        
        private static object CreateInstance(Type dependencyType)
        {
            object instance;

            if (InstantiatedInstanceDictionary.ContainsKey(dependencyType))
            {
                instance = InstantiatedInstanceDictionary[dependencyType];
            }
            else
            {
                instance = Activator.CreateInstance(dependencyType);
                InstantiatedInstanceDictionary[dependencyType] = instance;
            }

            return instance;
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
                var providerType = dependencyAttribute.ProvideType;

                if (DependencyDictionary.ContainsKey(dependencyType))
                    return;
                
                DependencyDictionary[dependencyType] = CreateInstance(providerType);
                DefaultDependencyTypeDictionary[dependencyType] = providerType;
            }
        }

        /// <summary>
        /// Sets the instance used for a dependency.
        /// </summary>
        /// <param name="instance">New instance</param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="ArgumentNullException">
        /// Raised if the argument is null
        /// </exception>
        public void SetDependency<T>([NotNull] T instance) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException("Dependency can not be set as null.");
            
            DependencyDictionary[typeof(T)] = instance;
        }

        /// <summary>
        /// Removes the instance used for a dependency, if one is set.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        public void ClearDependency<T>() where T : class
        {
            DependencyDictionary.Remove(typeof(T));
        }

        /// <summary>
        /// Restores the default dependency, which is the one
        /// set by <see cref="DependencyAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no default dependency type exists
        /// </exception>
        public void RestoreDefaultDependency<T>() where T : class
        {
            var dependencyType = typeof(T);
            
            if (!DefaultDependencyTypeDictionary.TryGetValue(dependencyType, out var providerType))
                throw new InvalidOperationException($"No default dependency type for {dependencyType}");

            DependencyDictionary[typeof(T)] = CreateInstance(providerType);
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

            if (!DependencyDictionary.ContainsKey(type))
                throw new MissingProviderException(type);
            
            return (T) DependencyDictionary[type];
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

            if (!DependencyDictionary.ContainsKey(type))
                return false;

            instance = (T) DependencyDictionary[type];

            return true;
        }
    }
}