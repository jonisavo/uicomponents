using System;
using UnityEditor;

namespace UIComponents.Core
{
    public abstract class PathAttribute : Attribute
    {
        public string Path { get; protected set; }

        public string GetAssetPathForComponent(UIComponent component)
        {
            if (Path == null)
                Path = string.Empty;

            if (!ConfiguredPathIsComplete())
            {
                if (TryGetPathFromComponent(component, out var pathFromComponent))
                    Path = pathFromComponent;
                else if (TryGetPathFromAssembly(component, out var pathFromAssembly))
                    Path = pathFromAssembly;
            }

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

            return TryGetValidAssetPathFromAttributes(component.AssetPathAttributes, out path);
        }

        private bool TryGetPathFromAssembly(UIComponent component, out string path)
        {
            path = "";
            
            var assembly = component.GetType().Assembly;

            var assetPathAttributes =
                assembly.GetCustomAttributes(typeof(AssetPathAttribute), false);

            return TryGetValidAssetPathFromAttributes((AssetPathAttribute[]) assetPathAttributes, out path);
        }

        private bool TryGetValidAssetPathFromAttributes(
            AssetPathAttribute[] attributes,
            out string path)
        {
            path = "";
            
            if (attributes.Length == 0)
                return false;

            foreach (var attribute in attributes)
            {
                var filePath = string.Join("/", attribute.Path, Path);

                if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(filePath)))
                    continue;

                path = filePath;

                return true;
            }

            return false;
        }
    }
}