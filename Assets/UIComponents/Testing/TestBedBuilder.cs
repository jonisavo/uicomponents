using System;

namespace UIComponents.Testing
{
    /// <summary>
    /// A class for configuring a <see cref="TestBed"/> instance with dependencies.
    /// </summary>
    public sealed class TestBedBuilder
    {
        private readonly TestBed _testBed;

        internal TestBedBuilder()
        {
            _testBed = new TestBed();
        }
        
        /// <summary>
        /// Add a singleton dependency to the test bed.
        /// </summary>
        /// <param name="value">Singleton value</param>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        public TestBedBuilder WithSingleton<TDependency>(TDependency value) where TDependency : class
        {
            _testBed.DiContainer.SetSingletonOverride(value);

            return this;
        }

        /// <summary>
        /// Add an override for a transient dependencies to the test bed.
        /// </summary>
        /// <param name="value">Transient dependency value</param>
        /// <typeparam name="TConsumer">Consumer type</typeparam>
        /// <typeparam name="TDependency">Dependency type</typeparam>
        public TestBedBuilder WithTransient<TConsumer, TDependency>(TDependency value)
            where TConsumer : class
            where TDependency : class
        {
            var injector = _testBed.DiContainer.GetInjector(typeof(TConsumer));
            
            injector.SetDependency(value, Scope.Transient);

            return this;
        }

        /// <summary>
        /// Set a timeout for async operations.
        /// </summary>
        /// <param name="timeout">Timeout for async operations</param>
        public TestBedBuilder WithAsyncTimeout(TimeSpan timeout)
        {
            _testBed.AsyncTimeout = timeout;

            return this;
        }
        
        /// <summary>
        /// Returns a <see cref="TestBed"/> instance with the configured dependencies.
        /// </summary>
        public TestBed Build()
        {
            return _testBed;
        }
    }
}
