using System;
using System.Collections.Generic;

namespace UIComponents.Core
{
    public class DependencyInjector
    {
        private static readonly Dictionary<Type, object> InstantiatedInstanceDictionary
            = new Dictionary<Type, object>();
        
        private readonly Dictionary<Type, object> _dependencyDictionary
            = new Dictionary<Type, object>();

        internal void AddProvidersFromDependencies(IEnumerable<InjectDependencyAttribute> dependencyAttributes)
        {
            foreach (var dependencyAttribute in dependencyAttributes)
            {
                var type = dependencyAttribute.DependencyType;
                var providerType = dependencyAttribute.ProviderType;
                
                if (!_dependencyDictionary.ContainsKey(type))
                    _dependencyDictionary[type] = CreateInstance(providerType);
            }
        }

        private static object CreateInstance(Type providerType)
        {
            object instance;

            if (InstantiatedInstanceDictionary.ContainsKey(providerType))
            {
                instance = InstantiatedInstanceDictionary[providerType];
            }
            else
            {
                instance = Activator.CreateInstance(providerType);
                InstantiatedInstanceDictionary[providerType] = instance;
            }

            return instance;
        }

        public void SetProvider<T>(T instance)
        {
            _dependencyDictionary[typeof(T)] = instance;
        }
        
        public T Provide<T>()
        {
            var type = typeof(T);

            if (!_dependencyDictionary.ContainsKey(type))
            {
                var value = (T) CreateInstance(type);
                SetProvider(value);
                return value;
            }

            return (T) _dependencyDictionary[type];
        }
    }
}