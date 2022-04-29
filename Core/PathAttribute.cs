using System;
using System.Collections.Generic;
using System.Linq;
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

            return TryGetValidAssetPath(component.GetAssetPaths(), out path);
        }

        private bool TryGetPathFromAssembly(UIComponent component, out string path)
        {
            path = "";
            
            var assembly = component.GetType().Assembly;

            var paths =
                assembly.GetCustomAttributes(typeof(AssetPathAttribute), false)
                    .Select(attribute => ((AssetPathAttribute)attribute).Path );

            return TryGetValidAssetPath(paths, out path);
        }

        private bool TryGetValidAssetPath(
            IEnumerable<string> paths,
            out string path)
        {
            path = "";

            foreach (var pathPart in paths)
            {
                var filePath = string.Join("/", pathPart, Path);

                if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(filePath)))
                    continue;

                path = filePath;

                return true;
            }

            return false;
        }
    }
}