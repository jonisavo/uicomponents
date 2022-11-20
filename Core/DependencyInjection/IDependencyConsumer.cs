using System.Collections.Generic;

namespace UIComponents.DependencyInjection
{
    public interface IDependencyConsumer
    {
        IEnumerable<IDependency> GetDependencies();
    }
}