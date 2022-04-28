using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Core
{
    public abstract class UIComponent : VisualElement
    {
        private readonly Type _componentType;

        private readonly LayoutAttribute _layoutAttribute;

        private readonly StylesheetAttribute[] _stylesheetAttributes;

        internal readonly AssetPathAttribute[] AssetPathAttributes;

        private static readonly Dictionary<Type, DependencyInjector> InjectorDictionary =
            new Dictionary<Type, DependencyInjector>();

        public static void SetDependencyProvider<TComponent, TDependency>(TDependency provider)
        {
            var componentType = typeof(TComponent);
            
            if (!InjectorDictionary.ContainsKey(componentType))
                InjectorDictionary.Add(componentType, new DependencyInjector());
            
            InjectorDictionary[componentType].SetProvider(provider);
        }

        protected UIComponent()
        {
            _componentType = GetType();
            _layoutAttribute = GetSingleAttribute<LayoutAttribute>();
            AssetPathAttributes = GetAttributes<AssetPathAttribute>();
            _stylesheetAttributes = GetAttributes<StylesheetAttribute>();

            var type = GetType();
            
            if (!InjectorDictionary.ContainsKey(type))
                InjectorDictionary.Add(type, new DependencyInjector());
            
            InjectorDictionary[type].AddProvidersFromDependencies(GetAttributes<InjectDependencyAttribute>());
            
            LoadLayout();
            LoadStyles();
        }

        protected T Provide<T>()
        {
            return InjectorDictionary[_componentType].Provide<T>();
        }

        [CanBeNull]
        protected virtual VisualTreeAsset GetLayout()
        {
            if (_layoutAttribute == null)
                return null;

            var assetPath = _layoutAttribute.GetAssetPathForComponent(this);
            
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
        }

        protected virtual StyleSheet[] GetStyleSheets()
        {
            var loadedStyleSheets = new StyleSheet[_stylesheetAttributes.Length];

            for (var i = 0; i < _stylesheetAttributes.Length; i++)
            {
                var assetPath = _stylesheetAttributes[i].GetAssetPathForComponent(this);
                var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(assetPath);

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
                styleSheets.Add(sheet);
        }
    }
}