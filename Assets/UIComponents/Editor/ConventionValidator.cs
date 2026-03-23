using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UIComponents.Editor
{
    /// <summary>
    /// Checks whether asset paths in the generated UIComponentAssetRegistry
    /// resolve to files on disk. Queries all registries across assemblies.
    /// <para/>
    /// For built-in sources this applies source-aware validation:
    /// <see cref="ResourcesAssetSource"/> uses <see cref="Resources.Load(string)"/>,
    /// and <see cref="AssetDatabaseAssetSource"/> also checks convention-based
    /// resolution through <see cref="AssetDatabase.FindAssets(string,string[])"/>.
    /// <para/>
    /// Custom sources may still resolve paths at runtime that this validator
    /// cannot verify in the editor.
    /// </summary>
    public static class ConventionValidator
    {
        private enum AssetSourceKind
        {
            Resources,
            AssetDatabase,
            Addressable,
            Custom
        }

        public struct ValidationResult
        {
            public Type ComponentType;
            public string AssetPath;
            public string AssetKind;
            public Type AssetSourceType;
            public bool Exists;
            public bool IsAmbiguous;
            public string ResolvedPath;
            public string[] CandidatePaths;
        }

        private struct RegistryAccessor
        {
            public MethodInfo TryGetEntry;
            public Type EntryType;
            public FieldInfo LayoutPathField;
            public FieldInfo StylesheetPathsField;
        }

        public static List<ValidationResult> ValidateAll()
        {
            var results = new List<ValidationResult>();

            var accessors = FindAllRegistryAccessors();
            if (accessors.Count == 0)
            {
                Debug.LogWarning("[UIComponents] No UIComponentAssetRegistry found. " +
                    "Ensure source generators have run.");
                return results;
            }

            var uiComponentType = typeof(UIComponent);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException) { continue; }

                foreach (var type in types)
                {
                    if (type.IsAbstract || !uiComponentType.IsAssignableFrom(type))
                        continue;

                    ValidateType(type, accessors, results);
                }
            }

            return results;
        }

        private static void ValidateType(
            Type type,
            List<RegistryAccessor> accessors,
            List<ValidationResult> results)
        {
            foreach (var accessor in accessors)
            {
                var args = new object[] { type, null };
                var found = (bool)accessor.TryGetEntry.Invoke(null, args);
                if (!found) continue;

                var entry = args[1];
                var layoutPath = (string)accessor.LayoutPathField.GetValue(entry);
                var stylesheetPaths = (string[])accessor.StylesheetPathsField.GetValue(entry);
                var assetSourceType = GetAssetSourceType(type);

                if (!string.IsNullOrEmpty(layoutPath))
                {
                    results.Add(ValidatePath(type, assetSourceType, layoutPath, "Layout"));
                }

                if (stylesheetPaths != null)
                {
                    foreach (var path in stylesheetPaths)
                    {
                        results.Add(ValidatePath(type, assetSourceType, path, "Stylesheet"));
                    }
                }

                return;
            }
        }

        private static ValidationResult ValidatePath(
            Type componentType,
            Type assetSourceType,
            string path,
            string assetKind)
        {
            var result = new ValidationResult
            {
                ComponentType = componentType,
                AssetPath = path,
                AssetKind = assetKind,
                AssetSourceType = assetSourceType
            };

            switch (GetAssetSourceKind(assetSourceType))
            {
                case AssetSourceKind.Resources:
                    result.Exists = ResourceExists(path);
                    result.ResolvedPath = result.Exists ? path : null;
                    return result;

                case AssetSourceKind.AssetDatabase:
                    return ValidateAssetDatabasePath(result);

                case AssetSourceKind.Addressable:
                case AssetSourceKind.Custom:
                default:
                    return ValidateFallbackPath(result);
            }
        }

        private static ValidationResult ValidateAssetDatabasePath(ValidationResult result)
        {
            if (TryGetDirectAssetDatabasePath(result.AssetPath, out var resolvedPath))
            {
                result.Exists = true;
                result.ResolvedPath = resolvedPath;
                return result;
            }

            var candidatePaths = FindConventionMatches(result.AssetPath, result.AssetKind);
            if (candidatePaths.Count == 1)
            {
                result.Exists = true;
                result.ResolvedPath = candidatePaths[0];
                return result;
            }

            if (candidatePaths.Count > 1)
            {
                result.IsAmbiguous = true;
                result.CandidatePaths = candidatePaths.ToArray();
            }

            return result;
        }

        private static ValidationResult ValidateFallbackPath(ValidationResult result)
        {
            if (TryGetDirectAssetDatabasePath(result.AssetPath, out var resolvedPath))
            {
                result.Exists = true;
                result.ResolvedPath = resolvedPath;
                return result;
            }

            result.Exists = ResourceExists(result.AssetPath);
            result.ResolvedPath = result.Exists ? result.AssetPath : null;
            return result;
        }

        private static bool ResourceExists(string path)
        {
            var resource = Resources.Load(path);
            if (resource != null)
            {
                Resources.UnloadAsset(resource);
                return true;
            }

            return false;
        }

        private static bool TryGetDirectAssetDatabasePath(string path, out string resolvedPath)
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

        private static List<string> FindConventionMatches(string path, string assetKind)
        {
            if (HasKnownAssetExtension(path))
                return new List<string>();

            var expectedExtension = assetKind == "Layout" ? ".uxml" : ".uss";
            var assetName = ExtractAssetName(path);
            var root = ExtractRoot(path, assetName);
            var searchFolders = string.IsNullOrEmpty(root)
                ? null
                : new[] { root };

            var candidatePaths = new List<string>();
            var guids = AssetDatabase.FindAssets(assetName, searchFolders);

            for (var i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (!Path.HasExtension(assetPath))
                    continue;

                if (!string.Equals(Path.GetExtension(assetPath), expectedExtension, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!string.Equals(Path.GetFileNameWithoutExtension(assetPath), assetName, StringComparison.Ordinal))
                    continue;

                candidatePaths.Add(assetPath);
            }

            candidatePaths.Sort(StringComparer.Ordinal);
            return candidatePaths;
        }

        private static bool HasKnownAssetExtension(string path)
        {
            return path.EndsWith(".uxml", StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith(".uss", StringComparison.OrdinalIgnoreCase);
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

        private static Type GetAssetSourceType(Type componentType)
        {
            var assetSourceType = GetAssemblyAssetSourceType(componentType.Assembly);
            var typeHierarchy = new Stack<Type>();

            for (var current = componentType; current != null; current = current.BaseType)
                typeHierarchy.Push(current);

            while (typeHierarchy.Count > 0)
            {
                var current = typeHierarchy.Pop();
                var currentAssetSourceType = GetDeclaredAssetSourceType(current);
                if (currentAssetSourceType != null)
                    assetSourceType = currentAssetSourceType;
            }

            return assetSourceType ?? typeof(ResourcesAssetSource);
        }

        private static Type GetAssemblyAssetSourceType(Assembly assembly)
        {
            var dependencyAttributes =
                assembly.GetCustomAttributes(typeof(DependencyAttribute), false);

            for (var i = 0; i < dependencyAttributes.Length; i++)
            {
                var attribute = dependencyAttributes[i] as DependencyAttribute;
                if (attribute != null && attribute.DependencyType == typeof(IAssetSource))
                    return attribute.ImplementationType;
            }

            return null;
        }

        private static Type GetDeclaredAssetSourceType(Type type)
        {
            var dependencyAttributes =
                type.GetCustomAttributes(typeof(DependencyAttribute), false);

            for (var i = 0; i < dependencyAttributes.Length; i++)
            {
                var attribute = dependencyAttributes[i] as DependencyAttribute;
                if (attribute != null && attribute.DependencyType == typeof(IAssetSource))
                    return attribute.ImplementationType;
            }

            return null;
        }

        private static AssetSourceKind GetAssetSourceKind(Type assetSourceType)
        {
            if (assetSourceType == null)
                return AssetSourceKind.Custom;

            if (typeof(ResourcesAssetSource).IsAssignableFrom(assetSourceType) ||
                assetSourceType.FullName == "UIComponents.ResourcesAssetResolver")
                return AssetSourceKind.Resources;

            if (typeof(AssetDatabaseAssetSource).IsAssignableFrom(assetSourceType) ||
                assetSourceType.FullName == "UIComponents.Editor.AssetDatabaseAssetResolver")
                return AssetSourceKind.AssetDatabase;

            if (assetSourceType.FullName == "UIComponents.Addressables.AddressableAssetSource" ||
                assetSourceType.FullName == "UIComponents.Addressables.AddressableAssetResolver")
                return AssetSourceKind.Addressable;

            return AssetSourceKind.Custom;
        }

        private static List<RegistryAccessor> FindAllRegistryAccessors()
        {
            var accessors = new List<RegistryAccessor>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var registryType = assembly.GetType("UIComponentAssetRegistry");
                if (registryType == null)
                    continue;

                var tryGetEntry = registryType.GetMethod("TryGetEntry",
                    BindingFlags.Public | BindingFlags.Static);
                var entryType = registryType.GetNestedType("AssetEntry");

                if (tryGetEntry == null || entryType == null)
                    continue;

                accessors.Add(new RegistryAccessor
                {
                    TryGetEntry = tryGetEntry,
                    EntryType = entryType,
                    LayoutPathField = entryType.GetField("LayoutPath"),
                    StylesheetPathsField = entryType.GetField("StylesheetPaths")
                });
            }

            return accessors;
        }

        [MenuItem("Window/UIComponents/Validate Registry Asset Paths")]
        private static void ValidateFromMenu()
        {
            var results = ValidateAll();
            var missingCount = 0;
            var ambiguousCount = 0;

            foreach (var result in results)
            {
                if (result.IsAmbiguous)
                {
                    var candidates = result.CandidatePaths == null
                        ? string.Empty
                        : string.Join(", ", result.CandidatePaths);
                    Debug.LogWarning(
                        $"[UIComponents] Ambiguous registry path ({result.AssetKind}) for {result.ComponentType.Name} " +
                        $"using {result.AssetSourceType?.Name ?? "unknown source"}: {result.AssetPath}. " +
                        $"Matches: {candidates}");
                    ambiguousCount++;
                    continue;
                }

                if (!result.Exists)
                {
                    Debug.LogWarning(
                        $"[UIComponents] Unresolved registry path ({result.AssetKind}) for {result.ComponentType.Name} " +
                        $"using {result.AssetSourceType?.Name ?? "unknown source"}: {result.AssetPath}");
                    missingCount++;
                }
            }

            if (missingCount == 0 && ambiguousCount == 0)
                Debug.Log($"[UIComponents] All {results.Count} registry asset paths resolved successfully.");
            else
                Debug.LogWarning(
                    $"[UIComponents] {missingCount} unresolved and {ambiguousCount} ambiguous registry path(s) " +
                    $"out of {results.Count} total. Components using custom or runtime-specific IAssetSource " +
                    "implementations may still resolve these paths outside editor validation.");
        }
    }
}
