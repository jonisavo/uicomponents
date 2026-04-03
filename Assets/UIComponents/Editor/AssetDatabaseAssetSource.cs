using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Editor
{
    /// <summary>
    /// An IAssetSource which loads assets with AssetDatabase.
    /// Convention paths (without file extensions) are resolved by
    /// searching within the asset root for matching assets.
    /// Explicit paths (with .uxml or .uss extensions) are loaded directly.
    /// Ambiguous convention matches are treated as failures.
    /// </summary>
    public class AssetDatabaseAssetSource : IAssetSource
    {
        private readonly Dictionary<string, AssetDatabasePathResolutionResult> _resolveCache =
            new Dictionary<string, AssetDatabasePathResolutionResult>();

        public Task<T> LoadAsset<T>(string assetPath) where T : Object
        {
            var resolution = ResolvePath(assetPath, AssetDatabasePathResolver.GetAssetKind(typeof(T)));
            if (!resolution.Exists)
                return Task.FromResult<T>(null);

            var asset = AssetDatabase.LoadAssetAtPath<T>(resolution.ResolvedPath);
            return Task.FromResult(asset);
        }

        public Task<bool> AssetExists(string assetPath)
        {
            var resolution = ResolvePath(assetPath, AssetDatabaseAssetKind.Any);
            return Task.FromResult(resolution.Exists);
        }

        private AssetDatabasePathResolutionResult ResolvePath(
            string path,
            AssetDatabaseAssetKind assetKind)
        {
            var cacheKey = assetKind + ":" + path;
            if (_resolveCache.TryGetValue(cacheKey, out var cached))
                return cached;

            var resolved = AssetDatabasePathResolver.Resolve(path, assetKind);
            _resolveCache[cacheKey] = resolved;
            return resolved;
        }
    }

    internal enum AssetDatabaseAssetKind
    {
        Any,
        Layout,
        Stylesheet
    }

    internal struct AssetDatabasePathResolutionResult
    {
        public bool Exists;
        public bool IsAmbiguous;
        public string ResolvedPath;
        public string[] CandidatePaths;
    }

    internal static class AssetDatabasePathResolver
    {
        public static AssetDatabaseAssetKind GetAssetKind(System.Type assetType)
        {
            if (typeof(VisualTreeAsset).IsAssignableFrom(assetType))
                return AssetDatabaseAssetKind.Layout;

            if (typeof(StyleSheet).IsAssignableFrom(assetType))
                return AssetDatabaseAssetKind.Stylesheet;

            return AssetDatabaseAssetKind.Any;
        }

        public static AssetDatabasePathResolutionResult Resolve(
            string path,
            AssetDatabaseAssetKind assetKind)
        {
            if (TryGetDirectAssetDatabasePath(path, out var resolvedPath) &&
                MatchesAssetKind(resolvedPath, assetKind))
            {
                return new AssetDatabasePathResolutionResult
                {
                    Exists = true,
                    ResolvedPath = resolvedPath
                };
            }

            var candidatePaths = FindConventionMatches(path, assetKind);
            
            return candidatePaths.Count switch
            {
                1 => new AssetDatabasePathResolutionResult { Exists = true, ResolvedPath = candidatePaths[0] },
                > 1 => new AssetDatabasePathResolutionResult
                {
                    IsAmbiguous = true, CandidatePaths = candidatePaths.ToArray()
                },
                _ => default(AssetDatabasePathResolutionResult)
            };
        }

        internal static bool MatchesAssetKind(string assetPath, AssetDatabaseAssetKind assetKind)
        {
            return assetKind switch
            {
                AssetDatabaseAssetKind.Any => true,
                AssetDatabaseAssetKind.Layout => string.Equals(Path.GetExtension(assetPath), ".uxml",
                    System.StringComparison.OrdinalIgnoreCase),
                AssetDatabaseAssetKind.Stylesheet => string.Equals(Path.GetExtension(assetPath), ".uss",
                    System.StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }

        private static List<string> FindConventionMatches(
            string path,
            AssetDatabaseAssetKind assetKind)
        {
            if (HasKnownAssetExtension(path))
                return new List<string>();

            var assetName = ExtractAssetName(path);
            var root = ExtractRoot(path, assetName);
            var searchFolders = string.IsNullOrEmpty(root)
                ? null
                : new[] { root };
            var expectedExtensions = GetExpectedExtensions(assetKind);

            var candidatePaths = new List<string>();
            var guids = AssetDatabase.FindAssets(assetName, searchFolders);

            for (var i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (!Path.HasExtension(assetPath))
                    continue;

                var extension = Path.GetExtension(assetPath);
                if (expectedExtensions != null && !expectedExtensions.Contains(extension))
                    continue;

                if (!string.Equals(Path.GetFileNameWithoutExtension(assetPath), assetName, System.StringComparison.Ordinal))
                    continue;

                candidatePaths.Add(assetPath);
            }

            candidatePaths.Sort(System.StringComparer.Ordinal);
            return candidatePaths;
        }

        private static HashSet<string> GetExpectedExtensions(AssetDatabaseAssetKind assetKind)
        {
            return assetKind switch
            {
                AssetDatabaseAssetKind.Layout => new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
                {
                    ".uxml"
                },
                AssetDatabaseAssetKind.Stylesheet => new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
                {
                    ".uss"
                },
                AssetDatabaseAssetKind.Any => new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
                {
                    ".uxml", ".uss"
                },
                _ => null
            };
        }

        private static bool HasKnownAssetExtension(string path)
        {
            return path.EndsWith(".uxml", System.StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith(".uss", System.StringComparison.OrdinalIgnoreCase);
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

        internal static bool TryGetDirectAssetDatabasePath(string path, out string resolvedPath)
        {
            resolvedPath = null;

            var guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid) || guid == "00000000000000000000000000000000")
                return false;

            if (AssetDatabase.IsValidFolder(path))
                return false;

            resolvedPath = path;
            return true;
        }
    }
}
