using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UIComponents.DependencyInjection;

namespace UIComponents.Testing
{
    /// <summary>
    /// <see cref="TestBed{TComponent}"/> is utility class for testing UIComponents with dependency injection.
    /// <para/>
    /// It allows overriding singleton and transient dependencies.
    /// <para/>
    /// </summary>
    public sealed class TestBed<TComponent> where TComponent : UIComponent
    {
        /// <summary>
        /// Timeout for async operations. Defaults to eight seconds.
        /// </summary>
        public TimeSpan AsyncTimeout { get; private set; } = TimeSpan.FromSeconds(8);

        internal readonly DiContext DiContext = new DiContext();

        private readonly Type _componentType = typeof(TComponent);

        /// <summary>
        /// Returns a dependency instance as requested by dependency type <typeparamref name="TDependency"/>.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <returns>Dependency instance</returns>
        /// <exception cref="MissingProviderException">Thrown if no provider exists</exception>
        public TDependency Provide<TDependency>() where TDependency : class
        {
            var injector = DiContext.GetInjector(_componentType);

            return injector.Provide<TDependency>();
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> using the given
        /// predicate.
        /// </summary>
        /// <param name="factoryPredicate">Predicate for creating the component</param>
        /// <returns>Component instance</returns>
        public TComponent CreateComponent(Func<TComponent> factoryPredicate)
        {
            var previousContext = DiContext.Current;

            DiContext.ChangeCurrent(DiContext);

            TComponent component;

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
        /// Creates a component of type <typeparamref name="TComponent"/> using the given
        /// predicate and waits for it to be initialized.
        /// </summary>
        /// <param name="factoryPredicate">Predicate for creating the component</param>
        /// <returns>Task that resolves to component instance when initialized</returns>
        /// <exception cref="TestBedTimeoutException">Thrown if component creation takes too long</exception>
        public async Task<TComponent> CreateComponentAsync(Func<TComponent> factoryPredicate)
        {
            var component = CreateComponent(factoryPredicate);
            component.Initialize();

            var initTask = component.InitializationTask;
            var timeoutTask = Task.Delay(AsyncTimeout);

            var task = await Task.WhenAny(initTask, timeoutTask);

            if (task == timeoutTask)
                throw new TestBedTimeoutException(component.GetType().Name, (int)AsyncTimeout.TotalMilliseconds);

            var initializedComponent = await initTask;

            return initializedComponent as TComponent;
        }


        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> with a default constructor.
        /// </summary>
        /// <returns>Component instance</returns>
        /// <exception cref="TestBedTimeoutException">Thrown if component creation takes too long</exception>
        public TComponent CreateComponent()
        {
            return CreateComponent(() => Activator.CreateInstance<TComponent>());
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> with a default constructor.
        /// and waits until for it to be initialized.
        /// </summary>
        /// <returns>Task that resolves to component instance when initialized</returns>
        /// <exception cref="TestBedTimeoutException">Thrown if component creation takes too long</exception>
        public async Task<TComponent> CreateComponentAsync()
        {
            return await CreateComponentAsync(() => Activator.CreateInstance<TComponent>());
        }

        /// <summary>
        /// Overrides a singleton dependency.
        /// </summary>
        /// <param name="value">New singleton value</param>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        public TestBed<TComponent> WithSingleton<TDependency>([NotNull] TDependency value)
            where TDependency : class
        {
            var injector = DiContext.GetInjector(_componentType);

            injector.SetSingletonOverride(value);

            return this;
        }

        /// <summary>
        /// Overrides a transient dependency.
        /// </summary>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        /// <param name="value">New transient value</param>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        public TestBed<TComponent> WithTransient<TDependency>([NotNull] TDependency value)
            where TDependency : class
        {
            var injector = DiContext.GetInjector(_componentType);

            injector.SetTransientInstance(value);

            return this;
        }

        /// <summary>
        /// Sets a new timeout for async operations.
        /// </summary>
        /// <param name="timeout">New async timeout</param>
        public TestBed<TComponent> WithAsyncTimeout(TimeSpan timeout)
        {
            AsyncTimeout = timeout;

            return this;
        }
    }
}
