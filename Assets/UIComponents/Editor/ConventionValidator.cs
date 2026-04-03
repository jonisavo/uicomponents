using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// and <see cref="AssetDatabaseAssetSource"/> uses the same convention
    /// resolution logic as runtime loading.
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
            public FieldInfo LayoutPathField;
            public FieldInfo StylesheetPathsField;
        }

        public static List<ValidationResult> ValidateAll()
        {
            var results = new List<ValidationResult>();
            AddressablesEditorPathResolver.Reset();

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
                    return ValidateAddressablePath(result);

                case AssetSourceKind.Custom:
                default:
                    return ValidateFallbackPath(result);
            }
        }

        private static ValidationResult ValidateAssetDatabasePath(ValidationResult result)
        {
            return ApplyResolution(
                result,
                AssetDatabasePathResolver.Resolve(
                    result.AssetPath,
                    GetAssetDatabaseAssetKind(result.AssetKind)));
        }

        private static ValidationResult ValidateAddressablePath(ValidationResult result)
        {
            return ApplyResolution(
                result,
                AddressablesEditorPathResolver.Resolve(
                    result.AssetPath,
                    GetAssetDatabaseAssetKind(result.AssetKind)));
        }

        private static ValidationResult ValidateFallbackPath(ValidationResult result)
        {
            if (AssetDatabasePathResolver.TryGetDirectAssetDatabasePath(result.AssetPath, out var resolvedPath))
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

        private static ValidationResult ApplyResolution(
            ValidationResult result,
            AssetDatabasePathResolutionResult resolution)
        {
            result.Exists = resolution.Exists;
            result.IsAmbiguous = resolution.IsAmbiguous;
            result.ResolvedPath = resolution.ResolvedPath;
            result.CandidatePaths = resolution.CandidatePaths;
            return result;
        }

        private static AssetDatabaseAssetKind GetAssetDatabaseAssetKind(string assetKind)
        {
            return assetKind == "Layout"
                ? AssetDatabaseAssetKind.Layout
                : AssetDatabaseAssetKind.Stylesheet;
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

    internal static class AddressablesEditorPathResolver
    {
        private static bool _initialized;
        private static bool _isAvailable;
        private static PropertyInfo _settingsProperty;
        private static MethodInfo _getAllAssetsMethod;
        private static PropertyInfo _assetPathProperty;
        private static MethodInfo _createKeyListMethod;
        private static Type _entryListType;
        private static object _indexedSettings;
        private static Dictionary<string, string[]> _layoutKeyIndex;
        private static Dictionary<string, string[]> _stylesheetKeyIndex;

        public static void Reset()
        {
            _indexedSettings = null;
            _layoutKeyIndex = null;
            _stylesheetKeyIndex = null;
        }

        public static AssetDatabasePathResolutionResult Resolve(
            string key,
            AssetDatabaseAssetKind assetKind)
        {
            EnsureInitialized();
            if (!_isAvailable)
                return default(AssetDatabasePathResolutionResult);

            var settings = _settingsProperty.GetValue(null, null);
            if (settings == null)
                return default(AssetDatabasePathResolutionResult);

            EnsureKeyIndex(settings);
            var keyIndex = GetKeyIndex(assetKind);
            if (keyIndex == null || !keyIndex.TryGetValue(key, out var candidatePaths))
                return default(AssetDatabasePathResolutionResult);

            if (candidatePaths.Length == 1)
            {
                return new AssetDatabasePathResolutionResult
                {
                    Exists = true,
                    ResolvedPath = candidatePaths[0]
                };
            }

            if (candidatePaths.Length > 1)
            {
                return new AssetDatabasePathResolutionResult
                {
                    IsAmbiguous = true,
                    CandidatePaths = candidatePaths
                };
            }

            return default(AssetDatabasePathResolutionResult);
        }

        private static IEnumerable<string> GetStringKeys(object entry)
        {
            var keys = _createKeyListMethod.Invoke(entry, null) as IEnumerable;
            if (keys == null)
                yield break;

            foreach (var candidate in keys)
            {
                if (candidate is string stringKey)
                    yield return stringKey;
            }
        }

        private static void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;

            var addressablesEditorAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetName().Name == "Unity.Addressables.Editor");
            if (addressablesEditorAssembly == null)
            {
                try
                {
                    addressablesEditorAssembly = Assembly.Load("Unity.Addressables.Editor");
                }
                catch
                {
                    return;
                }
            }

            var defaultObjectType =
                addressablesEditorAssembly.GetType("UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject");
            var settingsType =
                addressablesEditorAssembly.GetType("UnityEditor.AddressableAssets.Settings.AddressableAssetSettings");
            var entryType =
                addressablesEditorAssembly.GetType("UnityEditor.AddressableAssets.Settings.AddressableAssetEntry");

            if (defaultObjectType == null || settingsType == null || entryType == null)
                return;

            _settingsProperty = defaultObjectType.GetProperty(
                "Settings",
                BindingFlags.Public | BindingFlags.Static);
            _getAllAssetsMethod = settingsType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(method => method.Name == "GetAllAssets" &&
                    method.GetParameters().Length == 4);
            _assetPathProperty = entryType.GetProperty(
                "AssetPath",
                BindingFlags.Public | BindingFlags.Instance);
            _createKeyListMethod = entryType.GetMethod(
                "CreateKeyList",
                BindingFlags.Public | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            if (_settingsProperty == null ||
                _getAllAssetsMethod == null ||
                _assetPathProperty == null ||
                _createKeyListMethod == null)
                return;

            _entryListType = typeof(List<>).MakeGenericType(entryType);
            _isAvailable = true;
        }

        private static void EnsureKeyIndex(object settings)
        {
            if (ReferenceEquals(settings, _indexedSettings) &&
                _layoutKeyIndex != null &&
                _stylesheetKeyIndex != null)
                return;

            var layoutIndex = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var stylesheetIndex = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var entries = (IEnumerable)Activator.CreateInstance(_entryListType);

            _getAllAssetsMethod.Invoke(settings, new object[] { entries, false, null, null });

            foreach (var entry in entries)
            {
                var assetPath = _assetPathProperty.GetValue(entry, null) as string;
                if (string.IsNullOrEmpty(assetPath))
                    continue;

                var index = GetMutableKeyIndex(assetPath, layoutIndex, stylesheetIndex);
                if (index == null)
                    continue;

                foreach (var key in GetStringKeys(entry))
                    AddPath(index, key, assetPath);
            }

            _layoutKeyIndex = FinalizeIndex(layoutIndex);
            _stylesheetKeyIndex = FinalizeIndex(stylesheetIndex);
            _indexedSettings = settings;
        }

        private static Dictionary<string, string[]> GetKeyIndex(AssetDatabaseAssetKind assetKind)
        {
            switch (assetKind)
            {
                case AssetDatabaseAssetKind.Layout:
                    return _layoutKeyIndex;
                case AssetDatabaseAssetKind.Stylesheet:
                    return _stylesheetKeyIndex;
                default:
                    return null;
            }
        }

        private static Dictionary<string, HashSet<string>> GetMutableKeyIndex(
            string assetPath,
            Dictionary<string, HashSet<string>> layoutIndex,
            Dictionary<string, HashSet<string>> stylesheetIndex)
        {
            if (AssetDatabasePathResolver.MatchesAssetKind(assetPath, AssetDatabaseAssetKind.Layout))
                return layoutIndex;

            if (AssetDatabasePathResolver.MatchesAssetKind(assetPath, AssetDatabaseAssetKind.Stylesheet))
                return stylesheetIndex;

            return null;
        }

        private static void AddPath(
            Dictionary<string, HashSet<string>> index,
            string key,
            string assetPath)
        {
            if (!index.TryGetValue(key, out var paths))
            {
                paths = new HashSet<string>(StringComparer.Ordinal);
                index[key] = paths;
            }

            paths.Add(assetPath);
        }

        private static Dictionary<string, string[]> FinalizeIndex(
            Dictionary<string, HashSet<string>> index)
        {
            var finalized = new Dictionary<string, string[]>(index.Count, StringComparer.Ordinal);

            foreach (var pair in index)
            {
                var paths = pair.Value.ToArray();
                Array.Sort(paths, StringComparer.Ordinal);
                finalized[pair.Key] = paths;
            }

            return finalized;
        }
    }
}
