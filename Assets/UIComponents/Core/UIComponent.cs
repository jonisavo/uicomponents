using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UIComponents.Cache;
using UIComponents.Internal;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// A VisualElement which is configurable with various attributes like
    /// <see cref="LayoutAttribute"/> and <see cref="StylesheetAttribute"/>.
    /// <para/>
    /// Enables dependency injection with <see cref="DependencyAttribute"/>.
    /// </summary>
    /// <seealso cref="LayoutAttribute"/>
    /// <seealso cref="StylesheetAttribute"/>
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="ResourcesAssetResolver"/>
    [Dependency(typeof(IAssetResolver), provide: typeof(ResourcesAssetResolver))]
    public abstract class UIComponent : VisualElement
    {
        private static readonly Dictionary<Type, UIComponentCache> CacheDictionary =
            new Dictionary<Type, UIComponentCache>();
        
        /// <summary>
        /// Clears the cache of the UIComponent. Used primarily for testing.
        /// </summary>
        /// <typeparam name="TComponent">Component type</typeparam>
        public static void ClearCache<TComponent>() where TComponent : UIComponent
        {
            CacheDictionary.Remove(typeof(TComponent));
        }
        
        internal static bool TryGetCache<TComponent>(out UIComponentCache cache) where TComponent : UIComponent
        {
            return CacheDictionary.TryGetValue(typeof(TComponent), out cache);
        }

        /// <summary>
        /// The IAssetResolver used by this UIComponent.
        /// Defaults to <see cref="ResourcesAssetResolver"/>.
        /// </summary>
        public readonly IAssetResolver AssetResolver;

        private readonly DependencyInjector _dependencyInjector;

        private readonly Type _componentType;
        
        private static readonly ProfilerMarker DependencySetupProfilerMarker =
            new ProfilerMarker("UIComponent.DependencySetup");
        private static readonly ProfilerMarker CacheSetupProfilerMarker =
            new ProfilerMarker("UIComponent.CacheSetup");
        private static readonly ProfilerMarker LayoutAndStylesSetupProfilerMarker =
            new ProfilerMarker("UIComponent.LayoutAndStylesSetup");
        private static readonly ProfilerMarker QueryFieldsSetupProfilerMarker =
            new ProfilerMarker("UIComponent.QueryFieldsSetup");


        /// <summary>
        /// UIComponent's constructor loads the configured layout and stylesheets.
        /// </summary>
        protected UIComponent()
        {
            DependencySetupProfilerMarker.Begin();
            _componentType = GetType();
            _dependencyInjector = DependencyInjector.GetInjector(_componentType);
            DependencySetupProfilerMarker.End();

            AssetResolver = _dependencyInjector.Provide<IAssetResolver>();
            
            CacheSetupProfilerMarker.Begin();
            if (!CacheDictionary.ContainsKey(_componentType))
                CacheDictionary.Add(_componentType, new UIComponentCache(_componentType));
            CacheSetupProfilerMarker.End();

            LayoutAndStylesSetupProfilerMarker.Begin();
            LoadLayout();
            LoadStyles();
            LayoutAndStylesSetupProfilerMarker.End();
            QueryFieldsSetupProfilerMarker.Begin();
            PopulateQueryFields();
            QueryFieldsSetupProfilerMarker.End();
        }
        
        /// <summary>
        /// Returns an IEnumerable of all of the asset paths configured
        /// for the component.
        /// </summary>
        /// <returns>Asset paths of the component</returns>
        public IEnumerable<string> GetAssetPaths()
        {
            var assetPathAttributes = CacheDictionary[_componentType].AssetPathAttributes;
            var assetPathCount = assetPathAttributes.Count;

            for (var i = 0; i < assetPathCount; i++)
                yield return assetPathAttributes[i].Path;
        }

        /// <summary>
        /// Returns a dependency. Throws a <see cref="MissingProviderException"/>
        /// if the dependency can not be provided.
        /// </summary>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <exception cref="MissingProviderException">
        /// Thrown if the dependency can not be provided
        /// </exception>
        /// <returns>Dependency instance</returns>
        protected T Provide<T>() where T : class
        {
            return _dependencyInjector.Provide<T>();
        }

        /// <summary>
        /// Attempts to provide a dependency. Returns whether
        /// the dependency could be provided.
        /// </summary>
        /// <param name="instance">Dependency instance</param>
        /// <typeparam name="T">Dependency type</typeparam>
        /// <returns>Whether the dependency could be fetched</returns>
        protected bool TryProvide<T>(out T instance) where T : class
        {
            return _dependencyInjector.TryProvide(out instance);
        }
        
        [CanBeNull]
        private VisualTreeAsset GetLayout()
        {
            var layoutAttribute = CacheDictionary[_componentType].LayoutAttribute;
            
            if (layoutAttribute == null)
                return null;

            var assetPath = layoutAttribute.GetAssetPathForComponent(this);
            
            return AssetResolver.LoadAsset<VisualTreeAsset>(assetPath);
        }
        
        private StyleSheet[] GetStyleSheets()
        {
            var stylesheetAttributes = CacheDictionary[_componentType].StylesheetAttributes;
            var stylesheetAttributeCount = stylesheetAttributes.Count;
            var loadedStyleSheets = new StyleSheet[stylesheetAttributeCount];
            for (var i = 0; i < stylesheetAttributeCount; i++)
            {
                var assetPath = stylesheetAttributes[i].GetAssetPathForComponent(this);
                var styleSheet = AssetResolver.LoadAsset<StyleSheet>(assetPath);

                if (styleSheet == null)
                {
                    Debug.LogError($"Could not find stylesheet {assetPath} for {_componentType.Name}");
                    continue;
                }
                
                loadedStyleSheets[i] = styleSheet;
            }

            return loadedStyleSheets;
        }

        private void LoadLayout()
        {
            var layoutAsset = GetLayout();
            
            if (layoutAsset != null)
                layoutAsset.CloneTree(this);
        }

        private void LoadStyles()
        {
            var loadedStyleSheets = GetStyleSheets();
            var styleSheetCount = loadedStyleSheets.Length;

            if (styleSheetCount == 0)
                return;

            for (var i = 0; i < styleSheetCount; i++)
                if (loadedStyleSheets[i] != null)
                    styleSheets.Add(loadedStyleSheets[i]);
        }
        
        private void PopulateQueryFields()
        {
            var fieldCache = CacheDictionary[_componentType].FieldCache;
            var queryAttributeDictionary = fieldCache.QueryAttributes;

            foreach (var queryAttributeKeyPair in queryAttributeDictionary)
            {
                var fieldInfo = queryAttributeKeyPair.Key;
                var queryAttributes = queryAttributeKeyPair.Value;

                var fieldType = fieldInfo.FieldType;
                var concreteType = TypeUtils.GetConcreteType(fieldType);

                var results = new List<VisualElement>();
                
                for (var i = 0; i < queryAttributes.Length; i++)
                {
#if !UNITY_2020_3_OR_NEWER
                    if (queryAttributes[i].Name == null && queryAttributes[i].Class == null)
                    {
                        Unity2019CompatibilityUtils.QueryByDesiredType(queryAttributes[i], this, concreteType, results);
                        continue;
                    }
#endif
                    queryAttributes[i].Query(this, results);
                }
                
                results.RemoveAll(result => !concreteType.IsInstanceOfType(result));

                object value = null;

                if (fieldType.IsArray)
                    value = CollectionUtils.CreateArrayOfType(concreteType, results);
                else if (CollectionUtils.TypeQualifiesAsList(fieldType))
                    value = CollectionUtils.CreateListOfType(concreteType, results);
                else if (results.Count > 0)
                    value = results[0];

                if (value != null)
                    fieldInfo.SetValue(this, value);
            }
        }
    }
}