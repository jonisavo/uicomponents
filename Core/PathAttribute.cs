using System;
using System.Threading.Tasks;

namespace UIComponents
{
    /// <summary>
    /// A base class for attributes which store a path to an asset.
    /// </summary>
    public abstract class PathAttribute : Attribute
    {
        public string Path { get; protected set; }

        /// <summary>
        /// Returns the path stored by this attribute as it
        /// is used by the specified component.
        /// <para />
        /// If the path is not complete (i.e. an absolute path to
        /// an asset in Assets or Packages), the component's
        /// asset paths are prepended to this attribute's path.
        /// The first valid path is chosen.
        /// </summary>
        /// <param name="component">UIComponent to get path for</param>
        /// <returns>A Task which resolves to the asset path used by the component</returns>
        public async Task<string> GetAssetPathForComponent(UIComponent component)
        {
            if (Path == null)
                return string.Empty;

            var componentAssetPath = await GetPathFromComponent(component);

            if (!ConfiguredPathIsComplete() && !string.IsNullOrEmpty(componentAssetPath))
                return componentAssetPath;

            return Path;
        }

        private bool ConfiguredPathIsComplete()
        {
            return Path.StartsWith("Assets/") ||
                   Path.StartsWith("Packages/");
        }
        
        private async Task<string> GetPathFromComponent(UIComponent component)
        {
            foreach (var pathPart in component.GetAssetPaths())
            {
                var filePath = string.Join("/", pathPart, Path);

                var assetExists = await component.AssetResolver.AssetExists(filePath);
                
                if (!assetExists)
                    continue;

                return filePath;
            }

            return null;
        }
    }
}
