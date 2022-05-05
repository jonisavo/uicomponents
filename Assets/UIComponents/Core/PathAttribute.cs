using System;
using UnityEditor;
using UnityEngine;

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
        /// <returns>Asset path used by component</returns>
        public string GetAssetPathForComponent(UIComponent component)
        {
            if (Path == null)
                return string.Empty;

            if (!ConfiguredPathIsComplete() && TryGetPathFromComponent(component, out var path))
                return path;

            return Path;
        }

        private bool ConfiguredPathIsComplete()
        {
            return Path.StartsWith("Assets/") ||
                   Path.StartsWith("Packages/");
        }

        private bool TryGetPathFromComponent(UIComponent component, out string path)
        {
            path = "";

            foreach (var pathPart in component.GetAssetPaths())
            {
                var filePath = string.Join("/", pathPart, Path);

                if (!component.AssetResolver.AssetExists(filePath))
                    continue;

                path = filePath;

                return true;
            }

            return false;
        }
    }
}