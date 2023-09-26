using System;
using System.Collections.Generic;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// Represents a dependency injection context, which contains
    /// each consumer's dependency injectors and all singleton instances.
    /// </summary>
    public sealed class DiContext
    {
        /// <summary>
        /// The static dependency injection context.
        /// </summary>
        public static readonly DiContext Static = new DiContext();

        /// <summary>
        /// The current dependency injection context.
        /// </summary>
        public static DiContext Current { get; private set; } = Static;

        /// <summary>
        /// Changes the current dependency injection context.
        /// </summary>
        /// <param name="newContext">New context</param>
        /// <exception cref="ArgumentNullException">Thrown if the context is null</exception>

        public static void ChangeCurrent(DiContext newContext)
        {
            if (newContext == null)
                throw new ArgumentNullException(nameof(newContext));

            Current = newContext;
        }

        internal readonly Dictionary<Type, DependencyInjector> InjectorDictionary
            = new Dictionary<Type, DependencyInjector>();
        internal readonly Dictionary<Type, object> SingletonInstanceDictionary
            = new Dictionary<Type, object>();

        /// <summary>
        /// Returns a readonly dictionary of singleton instances, where the key
        /// is the type of the instance and the value is the instance itself.
        /// </summary>
        public IReadOnlyDictionary<Type, object> GetSingletonInstances()
        {
            return SingletonInstanceDictionary;
        }

        /// <summary>
        /// Clears the dependency injection context completely.
        /// </summary>
        public void Clear()
        {
            InjectorDictionary.Clear();
            SingletonInstanceDictionary.Clear();
        }

        /// <summary>
        /// Returns the dependency injector of the given consumer type.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        /// <returns>Consumer's dependency injector</returns>
        /// <exception cref="ArgumentNullException">Thrown if the consumer type is null</exception>
        public DependencyInjector GetInjector(Type consumerType)
        {
            if (consumerType == null)
                throw new ArgumentNullException(nameof(consumerType));
            
            if (InjectorDictionary.TryGetValue(consumerType, out var existingInjector))
                return existingInjector;

            var injector = new DependencyInjector(this);

            InjectorDictionary[consumerType] = injector;

            return injector;
        }

        /// <summary>
        /// Returns whether an injector exists for the given consumer type.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        public bool HasInjector(Type consumerType)
        {
            if (!InjectorDictionary.TryGetValue(consumerType, out var injector))
                return false;

            return injector.HasConsumer;
        }

        /// <summary>
        /// Adds a consumer to the dependency injection context. Its dependencies
        /// are added to the context if needed.
        /// </summary>
        /// <param name="consumer">Dependency consumer</param>
        public void RegisterConsumer(IDependencyConsumer consumer)
        {
            var consumerType = consumer.GetType();

            if (HasInjector(consumerType))
                return;

            var dependencies = consumer.GetDependencies();

            foreach (var dependency in dependencies)
            {
                if (dependency.GetScope() == Scope.Transient)
                    continue;

                var implementationType = dependency.GetImplementationType();

                if (SingletonInstanceDictionary.ContainsKey(implementationType))
                    continue;

                SingletonInstanceDictionary[implementationType] = dependency.CreateInstance();
            }

            var injector = GetInjector(consumerType);
            
            injector.SetConsumer(consumer);
        }
    }
}