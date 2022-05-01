using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UIComponents.Core.Exceptions;

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

        public static DependencyInjector GetInjector(Type consumerType)
        {
            if (InjectorDictionary.ContainsKey(consumerType))
                return InjectorDictionary[consumerType];

            return CreateInjector(consumerType);
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
        
        public DependencyInjector() {}

        public DependencyInjector(IEnumerable<DependencyAttribute> dependencyAttributes)
        {
            foreach (var dependencyAttribute in dependencyAttributes)
            {
                var type = dependencyAttribute.DependencyType;
                var providerType = dependencyAttribute.ProvideType;
                
                if (!DependencyDictionary.ContainsKey(type))
                    DependencyDictionary[type] = CreateInstance(providerType);
            }
        }

        public void SetDependency<T>([NotNull] T instance) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException("Dependency can not be set as null.");
            
            DependencyDictionary[typeof(T)] = instance;
        }

        [NotNull]
        public T Provide<T>() where T : class
        {
            var type = typeof(T);

            if (!DependencyDictionary.ContainsKey(type))
                throw new MissingProviderException(type);
            
            return (T) DependencyDictionary[type];
        }
        
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