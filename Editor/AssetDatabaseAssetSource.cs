using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UIComponents.Editor
{
    /// <summary>
    /// An IAssetSource which loads assets with AssetDatabase.
    /// Convention paths (without file extensions) are resolved by
    /// searching within the asset root for matching assets.
    /// Explicit paths (with .uxml or .uss extensions) are loaded directly.
    /// </summary>
    public class AssetDatabaseAssetSource : IAssetSource
    {
        private readonly Dictionary<string, string> _resolveCache =
            new Dictionary<string, string>();

        public Task<T> LoadAsset<T>(string assetPath) where T : Object
        {
            var resolved = ResolvePath(assetPath);
            var asset = AssetDatabase.LoadAssetAtPath<T>(resolved);
            return Task.FromResult(asset);
        }

        public Task<bool> AssetExists(string assetPath)
        {
            var resolved = ResolvePath(assetPath);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(resolved);
            return Task.FromResult(asset != null);
        }

        private string ResolvePath(string path)
        {
            if (path.EndsWith(".uxml") || path.EndsWith(".uss"))
                return path;

            if (_resolveCache.TryGetValue(path, out var cached))
                return cached;

            var assetName = ExtractAssetName(path);
            var root = ExtractRoot(path, assetName);
            var resolved = FindAsset(assetName, root) ?? path;

            _resolveCache[path] = resolved;
            return resolved;
        }

        private static string ExtractAssetName(string path)
        {
            var lastSlash = path.LastIndexOf('/');
            return lastSlash >= 0 ? path.Substring(lastSlash + 1) : path;
        }

        private static string ExtractRoot(string path, string assetName)
        {
            if (path.Length == assetName.Length)
                return null;

            return path.Substring(0, path.Length - assetName.Length).TrimEnd('/');
        }

        private static string FindAsset(string assetName, string root)
        {
            var searchFolders = string.IsNullOrEmpty(root)
                ? null
                : new[] { root };

            var guids = AssetDatabase.FindAssets(assetName, searchFolders);

            for (var i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (!Path.HasExtension(assetPath))
                    continue;
                var fileName = Path.GetFileNameWithoutExtension(assetPath);
                if (fileName == assetName)
                    return assetPath;
            }

            return null;
        }
    }
}
