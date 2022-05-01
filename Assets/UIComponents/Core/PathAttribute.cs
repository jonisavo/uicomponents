using System;
using UnityEditor;
using UnityEngine;

namespace UIComponents.Core
{
    public abstract class PathAttribute : Attribute
    {
        public string Path { get; protected set; }

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