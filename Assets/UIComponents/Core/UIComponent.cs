using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Core
{
    [Dependency(typeof(IAssetResolver), provide: typeof(ResourcesAssetResolver))]
    public abstract class UIComponent : VisualElement
    {
        private static readonly Dictionary<Type, LayoutAttribute> LayoutAttributeDictionary =
            new Dictionary<Type, LayoutAttribute>();
        private static readonly Dictionary<Type, StylesheetAttribute[]> StylesheetAttributesDictionary =
            new Dictionary<Type, StylesheetAttribute[]>();
        private static readonly Dictionary<Type, AssetPathAttribute[]> AssetPathAttributesDictionary =
            new Dictionary<Type, AssetPathAttribute[]>();
        
        internal readonly DependencyInjector DependencyInjector;

        internal readonly IAssetResolver AssetResolver;
        
        private readonly Type _componentType;

        protected UIComponent()
        {
            _componentType = GetType();
            DependencyInjector = DependencyInjector.GetInjector(_componentType);

            AssetResolver = DependencyInjector.Provide<IAssetResolver>();
            
            if (!LayoutAttributeDictionary.ContainsKey(_componentType))
                LayoutAttributeDictionary[_componentType] = GetSingleAttribute<LayoutAttribute>();
            
            if (!AssetPathAttributesDictionary.ContainsKey(_componentType))
                AssetPathAttributesDictionary[_componentType] = GetAttributes<AssetPathAttribute>();
            
            if (!StylesheetAttributesDictionary.ContainsKey(_componentType))
                StylesheetAttributesDictionary[_componentType] = GetAttributes<StylesheetAttribute>();

            LoadLayout();
            LoadStyles();
        }

        public IEnumerable<string> GetAssetPaths()
        {
            var assetPathAttributes = AssetPathAttributesDictionary[_componentType];

            foreach (var assetPathAttribute in assetPathAttributes)
                yield return assetPathAttribute.Path;
        }

        protected T Provide<T>() where T : class
        {
            return DependencyInjector.Provide<T>();
        }

        [CanBeNull]
        protected virtual VisualTreeAsset GetLayout()
        {
            var layoutAttribute = LayoutAttributeDictionary[_componentType];
            
            if (layoutAttribute == null)
                return null;

            var assetPath = layoutAttribute.GetAssetPathForComponent(this);
            
            return AssetResolver.LoadAsset<VisualTreeAsset>(assetPath);
        }

        protected virtual StyleSheet[] GetStyleSheets()
        {
            var stylesheetAttributes = StylesheetAttributesDictionary[_componentType];
            
            var loadedStyleSheets = new StyleSheet[stylesheetAttributes.Length];

            for (var i = 0; i < stylesheetAttributes.Length; i++)
            {
                var assetPath = stylesheetAttributes[i].GetAssetPathForComponent(this);
                var styleSheet = AssetResolver.LoadAsset<StyleSheet>(assetPath);

                if (styleSheet == null)
                {
                    Debug.LogError($"Could not find stylesheet {assetPath} for {GetType().Name}");
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
            else
                Debug.LogWarningFormat("Could not find layout for {0}", _componentType.Name);
        }

        private void LoadStyles()
        {
            var loadedStyleSheets = GetStyleSheets();

            if (loadedStyleSheets.Length == 0)
            {
                Debug.LogWarningFormat("Could not find styles for {0}", _componentType.Name);
                return;
            }
            
            foreach (var sheet in loadedStyleSheets)
                if (sheet != null)
                    styleSheets.Add(sheet);
        }
    }
}