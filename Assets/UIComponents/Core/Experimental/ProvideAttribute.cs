using System;
using JetBrains.Annotations;

namespace UIComponents.Experimental
{
    /// <summary>
    /// An attribute for specifying a field to be automatically
    /// assigned using dependency injection in UIComponents.
    /// If no provider exists for the dependency, the field
    /// is left unassigned and an error is logged.
    /// </summary>
    /// <example>
    /// [Dependency(typeof(ILogger), provide: typeof(MyLogger)]
    /// [Dependency(typeof(IDataService), provide: typeof(DataService)]
    /// public class ComponentWithDependencies : UIComponent
    /// {
    ///     [Provide]
    ///     private readonly ILogger Logger;
    ///
    ///     [Provide]
    ///     private readonly IDataService DataService;
    ///
    ///     [Provide(CastFrom = typeof(IDataService))]
    ///     private readonly DataService CastDataService;
    /// }
    /// </example>
    /// <seealso cref="DependencyAttribute"/>
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse]
    public class ProvideAttribute : Attribute
    {
        public Type CastFrom { get; set; }
    }
}
