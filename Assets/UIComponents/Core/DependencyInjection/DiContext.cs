using System;
using System.Collections.Generic;

namespace UIComponents.DependencyInjection
{
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

        public IReadOnlyDictionary<Type, object> GetSingletonInstances()
        {
            return SingletonInstanceDictionary;
        }

        public void Clear()
        {
            InjectorDictionary.Clear();
            SingletonInstanceDictionary.Clear();
        }

        public DependencyInjector GetInjector(Type consumerType)
        {
            if (consumerType == null)
                throw new ArgumentNullException(nameof(consumerType));
            
            if (InjectorDictionary.ContainsKey(consumerType))
                return InjectorDictionary[consumerType];

            var injector = new DependencyInjector(this);

            InjectorDictionary[consumerType] = injector;

            return injector;
        }

        private bool IsConsumerRegistered<T>() where T : IDependencyConsumer
        {
            if (!InjectorDictionary.TryGetValue(typeof(T), out var injector))
                return false;

            return injector.HasConsumer;
        }

        public void RegisterConsumer<T>(T consumer) where T : IDependencyConsumer
        {
            if (IsConsumerRegistered<T>())
                return;

            var dependencies = consumer.GetDependencies();

            foreach (var dependency in dependencies)
            {
                if (dependency.GetScope() == Scope.Transient)
                    continue;

                var dependencyType = dependency.GetDependencyType();

                if (SingletonInstanceDictionary.ContainsKey(dependencyType))
                    continue;

                SingletonInstanceDictionary[dependencyType] = dependency.CreateInstance();
            }
            
            var consumerType = typeof(T);

            var injector = GetInjector(consumerType);
            
            injector.SetConsumer(consumer);
        }

        public void SetSingleton<TDependency>(TDependency instance) where TDependency : class
        {
            SingletonInstanceDictionary[typeof(TDependency)] = instance;
        }
    }
}