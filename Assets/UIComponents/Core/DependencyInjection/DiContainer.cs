using System;
using System.Collections.Generic;

namespace UIComponents
{
    /// <summary>
    /// An internal class for storing injectors and singleton instances.
    /// </summary>
    internal class DiContainer
    {
        /// <summary>
        /// Contains all injectors created for consumers.
        /// </summary>
        internal readonly Dictionary<Type, DependencyInjector> InjectorDictionary;
        
        /// <summary>
        /// Contains the instantiated singleton dependencies which are consumed
        /// by interested parties.
        /// </summary>
        internal readonly Dictionary<Type, object> SingletonInstanceDictionary;

        public DiContainer()
        {
            InjectorDictionary =
                new Dictionary<Type, DependencyInjector>();
            
            SingletonInstanceDictionary
                = new Dictionary<Type, object>();
        }
        
        private DependencyInjector CreateInjector(Type consumerType)
        {
            var injectAttributes = (DependencyAttribute[])
                consumerType.GetCustomAttributes(typeof(DependencyAttribute), true);
            
            var injector = new DependencyInjector(injectAttributes);

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
            SingletonInstanceDictionary.Clear();
        }
        
        /// <summary>
        /// Attempts to fetch a singleton instance.
        /// </summary>
        /// <param name="type">Singleton type</param>
        /// <param name="instance">Singleton instance</param>
        /// <returns>Whether the instance could be fetched</returns>
        public bool TryGetSingletonInstance(Type type, out object instance)
        {
            return SingletonInstanceDictionary.TryGetValue(type, out instance);
        }

        private Dependency CreateSingletonDependency(Type dependencyType, Type providerType)
        {
            if (SingletonInstanceDictionary.TryGetValue(providerType, out var instance))
                return new Dependency(dependencyType, instance, Scope.Singleton);

            var dependency = new Dependency(dependencyType, providerType, Scope.Singleton);
            
            SingletonInstanceDictionary.Add(providerType, dependency.Instance);
            
            return dependency;
        }
        
        private Dependency CreateTransientDependency(Type dependencyType, Type providerType)
        {
            return new Dependency(dependencyType, providerType, Scope.Transient);
        }

        /// <summary>
        /// Creates a new Dependency object with the given dependency and provider types.
        /// </summary>
        /// <param name="dependencyType">Dependency type</param>
        /// <param name="providerType">Provider type</param>
        /// <param name="scope">Dependency scope</param>
        /// <returns>Dependency object</returns>
        public Dependency CreateDependency(Type dependencyType, Type providerType, Scope scope)
        {
            if (scope == Scope.Singleton)
                return CreateSingletonDependency(dependencyType, providerType);
            
            return CreateTransientDependency(dependencyType, providerType);
        }

        /// <summary>
        /// Creates a new Dependency object with the a dependency type and provided instance.
        /// </summary>
        /// <param name="dependencyType">Dependency type</param>
        /// <param name="instance">Provided instance</param>
        /// <param name="scope">Dependency scope</param>
        /// <returns>Dependency object</returns>
        public Dependency CreateDependency(Type dependencyType, object instance, Scope scope)
        {
            var instanceType = instance.GetType();
            
            if (scope == Scope.Singleton && !SingletonInstanceDictionary.ContainsKey(instanceType))
                SingletonInstanceDictionary.Add(instanceType, instance);
            
            return new Dependency(dependencyType, instance, scope);
        }
    }
}