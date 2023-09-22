using System;
using System.Collections.Generic;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// An abstract base class for classes which want to access
    /// UIComponents's dependency injection system.
    /// <example>
    /// <code>
    /// [Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
    /// public class MyClass : DependencyConsumer
    /// {
    ///     [Provide]
    ///     private IMyDependency _myDependency;
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="ProvideAttribute"/>
    /// </summary>
    public abstract class DependencyConsumer : IDependencyConsumer
    {
        private static readonly IDependency[] EmptyDependencies = Array.Empty<IDependency>();

        private readonly DependencyInjector _dependencyInjector;

        protected DependencyConsumer()
        {
            DiContext.Current.RegisterConsumer(this);
            _dependencyInjector = DiContext.Current.GetInjector(GetType());
            
            // ReSharper disable once VirtualMemberCallInConstructor
            UIC_PopulateProvideFields();
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
        protected T Provide<T>() where T : class
        {
            return _dependencyInjector.Provide<T>();
        }

        /// <summary>
        /// Attempts to provide a dependency. Returns whether
        /// the dependency could be provided.
        /// </summary>
        /// <param name="instance">Dependency instance</param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <returns>Whether the dependency could be fetched</returns>
        protected bool TryProvide<T>(out T instance) where T : class
        {
            return _dependencyInjector.TryProvide(out instance);
        }

        public virtual IEnumerable<IDependency> GetDependencies() => EmptyDependencies;
        
        protected virtual void UIC_PopulateProvideFields() {}
    }
}
