using System;
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

        protected UIComponent()
        {
            _componentType = GetType();
            _layoutAttribute = GetLayoutAttribute();
            _stylesheetAttributes = GetStylesheetAttributes();
            LoadLayout();
            LoadStyles();
        }

        [CanBeNull]
        private LayoutAttribute GetLayoutAttribute()
        {
            var layoutAttributes =
                (LayoutAttribute[]) _componentType.GetCustomAttributes(typeof(LayoutAttribute), true);

            if (layoutAttributes.Length == 0)
                return null;

            return layoutAttributes[0];
        }

        private StylesheetAttribute[] GetStylesheetAttributes()
        {
            return (StylesheetAttribute[])
                _componentType.GetCustomAttributes(typeof(StylesheetAttribute), true);
        }

        [CanBeNull]
        protected virtual VisualTreeAsset GetLayout()
        {
            if (_layoutAttribute == null)
                return null;
            
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_layoutAttribute.GetAssetPath());
        }

        protected virtual StyleSheet[] GetStyleSheets()
        {
            var loadedStyleSheets = new StyleSheet[_stylesheetAttributes.Length];

            for (var i = 0; i < _stylesheetAttributes.Length; i++)
            {
                var assetPath = _stylesheetAttributes[i].GetAssetPath();
                loadedStyleSheets[i] = AssetDatabase.LoadAssetAtPath<StyleSheet>(assetPath);
            }

            return loadedStyleSheets;
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