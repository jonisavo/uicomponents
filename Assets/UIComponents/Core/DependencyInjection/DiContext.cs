using System;
using JetBrains.Annotations;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// Dependency injection context for consumers.
    /// </summary>
    public sealed class DiContext
    {
        /// <summary>
        /// The current dependency injection context.
        /// </summary>
        [NotNull]
        public static DiContext Current { get; private set; } = new DiContext();

        /// <summary>
        /// The previous dependency injection context. Null if there is none.
        /// </summary>
        [CanBeNull]
        public static DiContext Previous { get; private set; } = null;
        
        /// <summary>
        /// Changes the current dependency injection context.
        /// </summary>
        /// <param name="context">New context</param>
        /// <exception cref="ArgumentNullException">Thrown if context is null</exception>
        public static void SetCurrent([NotNull] DiContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            
            if (context != Current)
                Previous = Current;
            
            Current = context;
        }
        
        public DiContainer Container { get; private set; }
        
        public DiContext()
        {
            Container = new DiContainer();
        }

        /// <summary>
        /// Change the DiContainer used in this context.
        /// </summary>
        /// <param name="container">New DiContainer</param>
        /// <exception cref="ArgumentNullException">Thrown if container is null</exception>
        public void SwitchContainer([NotNull] DiContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            
            Container = container;
        }
        
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
        public void SetDependency<TConsumer, TDependency>(TDependency provider)
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
        public void ClearDependency<TConsumer, TDependency>()
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));
            
            injector.ClearDependency<TDependency>();
        }
        
        /// <summary>
        /// Resets the provided instance of a dependency.
        /// If a singleton instance exists, it is restored.
        /// Otherwise, a new instance of the dependency is created.
        /// </summary>
        /// <remarks>
        /// Can be used in unit tests to clear
        /// restore dependency between or after tests.
        /// </remarks>
        /// <typeparam name="TConsumer">Consumer type</typeparam>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no configured dependency exists
        /// </exception>
        public void ResetProvidedInstance<TConsumer, TDependency>()
            where TConsumer : class
            where TDependency : class
        {
            var injector = GetInjector(typeof(TConsumer));
            
            injector.ResetProvidedInstance<TDependency>();
        }

        /// <summary>
        /// Returns the injector of the given consumer type.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        /// <returns>Injector of the consumer type</returns>
        public DependencyInjector GetInjector(Type consumerType)
        {
            return Container.GetInjector(consumerType);
        }
        
        /// <summary>
        /// Removes the injector of the given consumer type.
        /// Used primarily for testing.
        /// </summary>
        /// <param name="consumerType">Consumer type</param>
        public void RemoveInjector(Type consumerType)
        {
            Container.RemoveInjector(consumerType);
        }
    }
}
