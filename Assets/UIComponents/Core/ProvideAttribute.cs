using System;
using JetBrains.Annotations;

namespace UIComponents
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
        /// <summary>
        /// If set, an instance of the specified type will be provided and
        /// cast to the field type.
        /// </summary>
        public Type CastFrom { get; set; }
    }
}
