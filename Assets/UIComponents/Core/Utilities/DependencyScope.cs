using System;

namespace UIComponents.Utilities
{
    /// <summary>
    /// A helper class for setting a dependency temporarily.
    /// When disposed, restores the previous provider, or the lack thereof.
    /// Built for unit tests.
    /// </summary>
    /// <typeparam name="TConsumer">Type of the consumer</typeparam>
    /// <typeparam name="TDependency">Type of the dependency</typeparam>
    public class DependencyScope<TConsumer, TDependency> : IDisposable
        where TConsumer : class
        where TDependency : class
    {
        private readonly TDependency _previousDependencyProvider;
        
        public DependencyScope(TDependency instance)
        {
            var injector = DependencyInjector.GetInjector(typeof(TConsumer));

            if (injector.TryProvide<TDependency>(out var currentProvider))
                _previousDependencyProvider = currentProvider;
            
            DependencyInjector.SetDependency<TConsumer, TDependency>(instance);
        }
        
        public void Dispose()
        {
            if (_previousDependencyProvider == null)
                DependencyInjector.ClearDependency<TConsumer, TDependency>();
            else
                DependencyInjector.SetDependency<TConsumer, TDependency>(_previousDependencyProvider);
        }
    }
}