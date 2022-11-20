using System;

namespace UIComponents.DependencyInjection
{
    public interface IDependency
    {
        Type GetDependencyType();
        Type GetImplementationType();
        Scope GetScope();
        object CreateInstance();
    }
}