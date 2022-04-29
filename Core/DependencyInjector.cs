using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIComponents.Core
{
    public class DependencyInjector
    {
        internal static readonly Dictionary<Type, DependencyInjector> InjectorDictionary =
            new Dictionary<Type, DependencyInjector>();
        
        internal static readonly Dictionary<Type, object> InstantiatedInstanceDictionary
            = new Dictionary<Type, object>();
        
        internal readonly Dictionary<Type, object> DependencyDictionary
            = new Dictionary<Type, object>();

        public static void SetDependency<TConsumer, TDependency>(TDependency provider)
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));
            
            injector.SetDependency(provider);
        }

        public static TDependency GetDependency<TConsumer, TDependency>()
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));

            return injector.Provide<TDependency>();
        }

        public static DependencyInjector GetInjector(Type consumerType)
        {
            if (InjectorDictionary.ContainsKey(consumerType))
                return InjectorDictionary[consumerType];

            return CreateInjector(consumerType);
        }

        private static DependencyInjector CreateInjector(Type consumerType)
        {
            var injector = new DependencyInjector();

            var injectAttributes = (DependencyAttribute[])
                consumerType.GetCustomAttributes(typeof(DependencyAttribute), true);
            
            injector.PopulateFromDependencyAttributes(injectAttributes);
            
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

        public void SetDependency<T>(T instance) where T : class
        {
            DependencyDictionary[typeof(T)] = instance;
        }
        
        public T Provide<T>() where T : class
        {
            var type = typeof(T);
            
            if (DependencyDictionary.ContainsKey(type))
                return (T) DependencyDictionary[type];
            
            Debug.LogWarningFormat("Could not get dependency {0}", type.Name);
            
            return null;
        }
        
        private void PopulateFromDependencyAttributes(IEnumerable<DependencyAttribute> dependencyAttributes)
        {
            foreach (var dependencyAttribute in dependencyAttributes)
            {
                var type = dependencyAttribute.DependencyType;
                var providerType = dependencyAttribute.ProvideType;
                
                if (!DependencyDictionary.ContainsKey(type))
                    DependencyDictionary[type] = CreateInstance(providerType);
            }
        }
    }
}