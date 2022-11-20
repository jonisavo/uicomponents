using System;
using UnityEngine.TestTools;

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
    /// public partial class ComponentWithDependencies : UIComponent
    /// {
    ///     [Provide]
    ///     private ILogger _logger;
    ///
    ///     [Provide]
    ///     private IDataService _dataService;
    ///
    ///     [Provide(CastFrom = typeof(IDataService))]
    ///     private DataService _castDataService;
    /// }
    /// </example>
    /// <seealso cref="DependencyAttribute"/>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    [ExcludeFromCoverage]
    public sealed class ProvideAttribute : Attribute
    {
        /// <summary>
        /// If set, an instance of the specified type will be provided and
        /// cast to the field type.
        /// </summary>
        public Type CastFrom { get; set; }
    }
}
