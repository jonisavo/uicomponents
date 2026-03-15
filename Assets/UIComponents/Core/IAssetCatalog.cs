using System;

namespace UIComponents
{
    /// <summary>
    /// Resolves logical asset requests into concrete asset paths.
    /// Sits above <see cref="IAssetResolver"/>, which handles loading.
    /// </summary>
    public interface IAssetCatalog
    {
        /// <summary>
        /// Resolves a layout path for a component type.
        /// </summary>
        /// <param name="componentType">The type that declared the layout attribute</param>
        /// <param name="path">The path computed by the source generator</param>
        /// <returns>The resolved path to pass to IAssetResolver</returns>
        string ResolveLayoutPath(Type componentType, string path);

        /// <summary>
        /// Resolves a stylesheet path for a component type.
        /// </summary>
        /// <param name="componentType">The type that declared the stylesheet attribute</param>
        /// <param name="path">The path computed by the source generator</param>
        /// <returns>The resolved path to pass to IAssetResolver</returns>
        string ResolveStylesheetPath(Type componentType, string path);

        /// <summary>
        /// Resolves a shared stylesheet path.
        /// </summary>
        /// <param name="name">The logical shared stylesheet name</param>
        /// <param name="path">The path computed by the source generator</param>
        /// <returns>The resolved path to pass to IAssetResolver</returns>
        string ResolveSharedStylesheetPath(string name, string path);
    }
}
