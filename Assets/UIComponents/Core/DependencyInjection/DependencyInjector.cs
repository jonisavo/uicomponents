using System;
using System.Collections.Generic;

namespace UIComponents.DependencyInjection
{
    public sealed class DependencyInjector
    {
        public bool HasConsumer { get; private set; }
        
        private readonly DiContext _diContext;
        private readonly IReadOnlyDictionary<Type, object> _singletonInstanceDictionary;
        
        private IDependencyConsumer _consumer;
        private Dictionary<Type, object> _transientInstanceDictionary
            = new Dictionary<Type, object>();

        public DependencyInjector(DiContext context)
        {
            _diContext = context;
            _singletonInstanceDictionary = context.GetSingletonInstances();
        }

        public DependencyInjector(IDependencyConsumer consumer, DiContext context) : this(context)
        {
            SetConsumer(consumer);
        }

        public void SetConsumer(IDependencyConsumer consumer)
        {
            _consumer = consumer;
            
            _transientInstanceDictionary.Clear();

            foreach (var dependency in consumer.GetDependencies())
            {
                if (dependency.GetScope() == Scope.Singleton)
                    continue;

                var dependencyType = dependency.GetDependencyType();

                if (_transientInstanceDictionary.ContainsKey(dependencyType))
                    continue;

                _transientInstanceDictionary[dependencyType] = dependency.CreateInstance();
            }

            HasConsumer = true;
        }

        public T Provide<T>() where T : class
        {
            var couldProvide = TryProvide<T>(out var instance);
            
            if (!couldProvide)
                throw new MissingProviderException(typeof(T));

            return instance;
        }

        public bool TryProvide<T>(out T instance) where T : class
        {
            var dependencyType = typeof(T);
            
            instance = null;
            
            if (_singletonInstanceDictionary.ContainsKey(dependencyType))
                instance = _singletonInstanceDictionary[dependencyType] as T;

            if (_transientInstanceDictionary.ContainsKey(dependencyType))
                instance = _transientInstanceDictionary[dependencyType] as T;

            return instance != null;
        }

        public void SetTransientInstance<TDependency>(TDependency instance) where TDependency : class
        {
            _transientInstanceDictionary[typeof(TDependency)] = instance;
        }
    }
}