using System;
using JetBrains.Annotations;
using UIComponents.DependencyInjection;

namespace UIComponents.Testing
{
    /// <summary>
    /// <see cref="TestBed{TConsumer}"/> is utility class for working with
    /// UIComponent's built-in dependency injection in unit tests.
    /// <para/>
    /// It allows overriding singleton and transient dependencies
    /// before constructing instances.
    /// <para/>
    /// </summary>
    public sealed class TestBed<TConsumer>
    {
        internal readonly DiContext DiContext;
        private readonly DependencyInjector _injector;

        public TestBed()
        {
            DiContext = new DiContext();
            _injector = DiContext.GetInjector(typeof(TConsumer));
        }

        /// <summary>
        /// Returns a dependency instance of type <typeparamref name="TDependency"/>.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <returns>Dependency instance</returns>
        /// <exception cref="MissingProviderException">Thrown if no provider exists</exception>
        public TDependency Provide<TDependency>() where TDependency : class
        {
            return _injector.Provide<TDependency>();
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TConsumer"/> using the given
        /// predicate.
        /// </summary>
        /// <param name="factoryPredicate">Predicate for creating the component</param>
        private TConsumer InstantiateWithPredicate(Func<TConsumer> factoryPredicate)
        {
            var previousContext = DiContext.Current;

            DiContext.ChangeCurrent(DiContext);

            TConsumer component;

            try
            {
                component = factoryPredicate();
            }
            finally
            {
                DiContext.ChangeCurrent(previousContext);
            }

            return component;
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TConsumer"/> with a default constructor.
        /// </summary>
        public TConsumer Instantiate()
        {
            return InstantiateWithPredicate(Activator.CreateInstance<TConsumer>);
        }

        /// <summary>
        /// Overrides a singleton dependency.
        /// </summary>
        /// <param name="value">New singleton value</param>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        public TestBed<TConsumer> WithSingleton<TDependency>([NotNull] TDependency value)
            where TDependency : class
        {
            _injector.SetSingletonOverride(value);

            return this;
        }

        /// <summary>
        /// Overrides a transient dependency.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <param name="value">New transient value</param>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        public TestBed<TConsumer> WithTransient<TDependency>([NotNull] TDependency value)
            where TDependency : class
        {
            _injector.SetTransientInstance(value);

            return this;
        }
    }
}
