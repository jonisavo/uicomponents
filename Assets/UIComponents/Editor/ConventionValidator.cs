using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UIComponents.Editor
{
    /// <summary>
    /// Checks whether asset paths in the generated UIComponentAssetRegistry
    /// resolve to files on disk. Queries all registries across assemblies.
    /// <para/>
    /// This checks the raw paths stored in the registry (as computed by source
    /// generators). If a component uses a custom <see cref="IAssetCatalog"/>
    /// that remaps paths at runtime, those remapped paths are not checked here.
    /// </summary>
    public static class ConventionValidator
    {
        public struct ValidationResult
        {
            public Type ComponentType;
            public string AssetPath;
            public string AssetKind;
            public bool Exists;
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

                if (!string.IsNullOrEmpty(layoutPath))
                {
                    results.Add(new ValidationResult
                    {
                        ComponentType = type,
                        AssetPath = layoutPath,
                        AssetKind = "Layout",
                        Exists = AssetExists(layoutPath)
                    });
                }

                if (stylesheetPaths != null)
                {
                    foreach (var path in stylesheetPaths)
                    {
                        results.Add(new ValidationResult
                        {
                            ComponentType = type,
                            AssetPath = path,
                            AssetKind = "Stylesheet",
                            Exists = AssetExists(path)
                        });
                    }
                }

                return;
            }
        }

        private static bool AssetExists(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (!string.IsNullOrEmpty(guid) && guid != "00000000000000000000000000000000")
                return true;

            var resource = Resources.Load(path);
            if (resource != null)
            {
                Resources.UnloadAsset(resource);
                return true;
            }

            return false;
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

        [MenuItem("UIComponents/Validate Registry Asset Paths")]
        private static void ValidateFromMenu()
        {
            var results = ValidateAll();
            var missingCount = 0;

            foreach (var result in results)
            {
                if (!result.Exists)
                {
                    Debug.LogWarning(
                        $"[UIComponents] Unresolved registry path ({result.AssetKind}) for {result.ComponentType.Name}: {result.AssetPath}");
                    missingCount++;
                }
            }

            if (missingCount == 0)
                Debug.Log($"[UIComponents] All {results.Count} registry asset paths resolved successfully.");
            else
                Debug.LogWarning(
                    $"[UIComponents] {missingCount} unresolved registry path(s) out of {results.Count} total. " +
                    "Components using a custom IAssetCatalog may resolve these paths at runtime.");
        }
    }
}
