using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// A class for storing injectors and singleton instances.
    /// </summary>
    public class DiContainer
    {
        /// <summary>
        /// Contains all injectors created for consumers.
        /// </summary>
        internal readonly Dictionary<Type, DependencyInjector> InjectorDictionary;
        
        /// <summary>
        /// Contains the instantiated singleton dependencies which are consumed
        /// by interested parties.
        /// The key is the type of the instantiated value.
        /// </summary>
        private readonly Dictionary<Type, object> _singletonInstanceDictionary;

        /// <summary>
        /// Contains singletons which are overridden for testing purposes.
        /// The key is the type of dependency and the value is the singleton instance.
        /// </summary>
        private readonly Dictionary<Type, object> _singletonOverrideDictionary;

        public DiContainer()
        {
            InjectorDictionary = new Dictionary<Type, DependencyInjector>();
            
            _singletonInstanceDictionary = new Dictionary<Type, object>();

            _singletonOverrideDictionary = new Dictionary<Type, object>();
        }
        
        private DependencyInjector CreateInjector(Type consumerType)
        {
            var dependencyAttributes = (DependencyAttribute[])
                consumerType.GetCustomAttributes(typeof(DependencyAttribute), true);

            var injector = new DependencyInjector(dependencyAttributes, this);

            InjectorDictionary.Add(consumerType, injector);

            return injector;
        }

        /// <summary>
        /// Returns the consumer's dependency injector, creating
        /// one if it does not exist.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        /// <returns>Dependency injector</returns>
        public DependencyInjector GetInjector(Type consumerType)
        {
            if (InjectorDictionary.ContainsKey(consumerType))
                return InjectorDictionary[consumerType];

            return CreateInjector(consumerType);
        }
        
        /// <summary>
        /// Removes the consumer's dependency injector.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        public void RemoveInjector(Type consumerType)
        {
            InjectorDictionary.Remove(consumerType);
        }

        /// <summary>
        /// Clears the container.
        /// </summary>
        public void Clear()
        {
            InjectorDictionary.Clear();
            _singletonInstanceDictionary.Clear();
        }
        
        /// <summary>
        /// Attempts to fetch a singleton instance.
        /// </summary>
        /// <param name="type">Singleton type</param>
        /// <param name="instance">Singleton instance</param>
        /// <returns>Whether the instance could be fetched</returns>
        public bool TryGetSingletonInstance(Type type, out object instance)
        {
            return _singletonInstanceDictionary.TryGetValue(type, out instance);
        }

        /// <summary>
        /// Attempts to fetch a singleton instance.
        /// </summary>
        /// <typeparam name="T">Singleton type</typeparam>
        /// <param name="instance">Singleton instance</param>
        /// <returns>Whether the instance could be fetched</returns>
        public bool TryGetSingletonInstance<T>(out T instance) where T : class
        {
            instance = null;
            
            if (!_singletonInstanceDictionary.TryGetValue(typeof(T), out var obj))
                return false;

            instance = obj as T;

            return instance != null;
        }
        
        /// <param name="instanceType">Type of the singleton instance</param>
        /// <param name="instance">Singleton instance to add</param>
        /// <exception cref="ArgumentNullException">Thrown if instance is null</exception>
        public void RegisterSingletonInstance(Type instanceType, [NotNull] object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            
            _singletonInstanceDictionary[instanceType] = instance;
        }
        
        /// <param name="type">Singleton type</param>
        /// <returns>Whether an instance is contained in the container</returns>
        public bool ContainsSingletonInstanceOfType(Type type)
        {
            return _singletonInstanceDictionary.ContainsKey(type);
        }

        /// <summary>
        /// Overrides a singleton dependency.
        /// </summary>
        /// <param name="value">New singleton value</param>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        public void SetSingletonOverride<TDependency>([NotNull] TDependency value) where TDependency : class
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            _singletonOverrideDictionary[typeof(TDependency)] = value;
            
            foreach (var injector in InjectorDictionary.Values)
                injector.ResetProvidedInstance<TDependency>();
        }
        
        /// <summary>
        /// Removes any set singleton override.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        public void RemoveSingletonOverride<TDependency>() where TDependency : class
        {
            _singletonOverrideDictionary.Remove(typeof(TDependency));
            
            foreach (var injector in InjectorDictionary.Values)
                injector.ResetProvidedInstance<TDependency>();
        }
        
        /// <summary>
        /// Tries to get a set singleton override.
        /// </summary>
        /// <param name="dependencyType">Dependency type</param>
        /// <param name="value">Singleton value</param>
        /// <returns>Whether the singleton override could be fetched</returns>
        public bool TryGetSingletonOverride(Type dependencyType, out object value)
        {
            return _singletonOverrideDictionary.TryGetValue(dependencyType, out value);
        }
    }
}
