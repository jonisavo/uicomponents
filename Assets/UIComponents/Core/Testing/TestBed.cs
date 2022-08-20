using System;
using UIComponents.DependencyInjection;

namespace UIComponents.Testing
{
    /// <summary>
    /// <see cref="TestBed"/> is utility class for testing UIComponents with dependency injection.
    /// <para/>
    /// It allows overriding singleton and transient dependencies.
    /// <para/>
    /// You can initialize an instance with <see cref="Create"/>.
    /// </summary>
    public class TestBed
    {
        internal readonly DiContainer DiContainer = new DiContainer();
        
        internal TestBed() {}
        
        /// <summary>
        /// Starts the configuration of a test bed.
        /// </summary>
        public static TestBedBuilder Create()
        {
            return new TestBedBuilder();
        }

        /// <summary>
        /// Returns a dependency instance as requested by consumer type <typeparamref name="TConsumer"/>.
        /// </summary>
        /// <typeparam name="TConsumer">Consumer type</typeparam>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <returns>Dependency instance</returns>
        /// <exception cref="MissingProviderException">Thrown if no provider exists</exception>
        public TDependency Provide<TConsumer, TDependency>()
            where TConsumer : class
            where TDependency : class
        {
            var injector = DiContainer.GetInjector(typeof(TConsumer));

            return injector.Provide<TDependency>();
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> using the given
        /// predicate.
        /// </summary>
        /// <param name="factoryPredicate">Predicate for creating the component</param>
        /// <typeparam name="TComponent">Component type</typeparam>
        /// <returns>Component instance</returns>
        public TComponent CreateComponent<TComponent>(Func<TComponent> factoryPredicate)
            where TComponent : UIComponent
        {
            var previousContainer = DiContext.Current.Container;
            
            DiContext.Current.SwitchContainer(DiContainer);

            TComponent component;
            
            try
            {
                component = factoryPredicate();
            }
            finally
            {
                DiContext.Current.SwitchContainer(previousContainer);
            }

            return component;
        }

        
        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> with a default constructor.
        /// </summary>
        /// <typeparam name="TComponent">Component type</typeparam>
        /// <returns>Component instance</returns>
        public TComponent CreateComponent<TComponent>() where TComponent : UIComponent, new()
        {
            return CreateComponent(() => new TComponent());
        }
    }
}
