using System;
using System.Collections.Generic;
using UIComponents.Core.Exceptions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Core
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
        private static readonly Dictionary<Type, LayoutAttribute> LayoutAttributeDictionary =
            new Dictionary<Type, LayoutAttribute>();
        private static readonly Dictionary<Type, StylesheetAttribute[]> StylesheetAttributesDictionary =
            new Dictionary<Type, StylesheetAttribute[]>();
        private static readonly Dictionary<Type, AssetPathAttribute[]> AssetPathAttributesDictionary =
            new Dictionary<Type, AssetPathAttribute[]>();
        
        /// <summary>
        /// The IAssetResolver used by this UIComponent.
        /// Defaults to <see cref="ResourcesAssetResolver"/>.
        /// </summary>
        public readonly IAssetResolver AssetResolver;

        private readonly DependencyInjector _dependencyInjector;

        private readonly Type _componentType;

        /// <summary>
        /// UIComponent's constructor loads the configured layout and stylesheets.
        /// </summary>
        protected UIComponent()
        {
            _componentType = GetType();
            _dependencyInjector = DependencyInjector.GetInjector(_componentType);

            AssetResolver = _dependencyInjector.Provide<IAssetResolver>();
            
            if (!LayoutAttributeDictionary.ContainsKey(_componentType))
                LayoutAttributeDictionary[_componentType] = GetSingleAttribute<LayoutAttribute>();
            
            if (!AssetPathAttributesDictionary.ContainsKey(_componentType))
                AssetPathAttributesDictionary[_componentType] = GetAttributes<AssetPathAttribute>();
            
            if (!StylesheetAttributesDictionary.ContainsKey(_componentType))
                StylesheetAttributesDictionary[_componentType] = GetAttributes<StylesheetAttribute>();

            LoadLayout();
            LoadStyles();
        }
        
        /// <summary>
        /// Returns an IEnumerable of all of the asset paths configured
        /// for the component.
        /// </summary>
        /// <returns>Asset paths of the component</returns>
        public IEnumerable<string> GetAssetPaths()
        {
            var assetPathAttributes = AssetPathAttributesDictionary[_componentType];

            foreach (var assetPathAttribute in assetPathAttributes)
                yield return assetPathAttribute.Path;
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
            var layoutAttribute = LayoutAttributeDictionary[_componentType];
            
            if (layoutAttribute == null)
                return null;

            var assetPath = layoutAttribute.GetAssetPathForComponent(this);
            
            return AssetResolver.LoadAsset<VisualTreeAsset>(assetPath);
        }
        
        private StyleSheet[] GetStyleSheets()
        {
            var stylesheetAttributes = StylesheetAttributesDictionary[_componentType];
            
            var loadedStyleSheets = new StyleSheet[stylesheetAttributes.Length];

            for (var i = 0; i < stylesheetAttributes.Length; i++)
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

        [CanBeNull]
        private T GetSingleAttribute<T>() where T : Attribute
        {
            var layoutAttributes = GetAttributes<T>();

            if (layoutAttributes.Length == 0)
                return null;

            return layoutAttributes[0];
        }

        private T[] GetAttributes<T>() where T : Attribute
        {
            return (T[]) _componentType.GetCustomAttributes(typeof(T), true);
        }

        private void LoadLayout()
        {
            var layoutAsset = GetLayout();
            
            if (layoutAsset)
                layoutAsset.CloneTree(this);
        }

        private void LoadStyles()
        {
            var loadedStyleSheets = GetStyleSheets();

            if (loadedStyleSheets.Length == 0)
                return;

            foreach (var sheet in loadedStyleSheets)
                if (sheet != null)
                    styleSheets.Add(sheet);
        }
    }
}