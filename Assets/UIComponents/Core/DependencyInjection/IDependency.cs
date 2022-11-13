using System;

namespace UIComponents.DependencyInjection
{
    public interface IDependency
    {
        Type GetDependencyType();
        Scope GetScope();
        object CreateInstance();
    }
}