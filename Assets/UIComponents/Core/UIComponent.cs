using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UIComponents.Cache;
using UIComponents.DependencyInjection;
using UIComponents.Internal;
using Unity.Profiling;
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
    /// <seealso cref="UIComponentDebugLogger"/>
    [Dependency(typeof(IAssetResolver), provide: typeof(ResourcesAssetResolver))]
    [Dependency(typeof(IUIComponentLogger), provide: typeof(UIComponentDebugLogger))]
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

        /// <summary>
        /// Whether the UIComponent has been fully initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// A Task that completes when the UIComponent has been fully initialized.
        /// </summary>
        public Task<UIComponent> InitializationTask => _initCompletionSource.Task;

        /// <summary>
        /// The IUIComponentLogger used by this UIComponent.
        /// Defaults to <see cref="UIComponentDebugLogger"/>.
        /// </summary>
        protected readonly IUIComponentLogger Logger;

        private readonly DependencyInjector _dependencyInjector;

        private readonly Type _componentType;

        private readonly TaskCompletionSource<UIComponent> _initCompletionSource =
            new TaskCompletionSource<UIComponent>();

        private static readonly ProfilerMarker DependencySetupProfilerMarker =
            new ProfilerMarker("UIComponent.DependencySetup");
        private static readonly ProfilerMarker CacheSetupProfilerMarker =
            new ProfilerMarker("UIComponent.CacheSetup");
        private static readonly ProfilerMarker PostHierarchySetupProfilerMarker =
            new ProfilerMarker("UIComponent.PostHierarchySetup");

        /// <summary>
        /// UIComponent's constructor loads the configured layout and stylesheets.
        /// </summary>
        protected UIComponent()
        {
            CacheSetupProfilerMarker.Begin();

            _componentType = GetType();
            if (!CacheDictionary.ContainsKey(_componentType))
                CacheDictionary.Add(_componentType, new UIComponentCache(_componentType));

            CacheSetupProfilerMarker.End();

            DependencySetupProfilerMarker.Begin();

            _dependencyInjector = DiContext.Current.GetInjector(_componentType);
            AssetResolver = Provide<IAssetResolver>();
            Logger = Provide<IUIComponentLogger>();
            PopulateProvideFields();

            DependencySetupProfilerMarker.End();

            Initialize();
        }

        private async void Initialize()
        {
            var layoutTask = GetLayout();
            var stylesTask = GetStyleSheets();

            await Task.WhenAll(layoutTask, stylesTask);

            var layoutAsset = layoutTask.Result;
            var styles = stylesTask.Result;

            LoadLayout(layoutAsset);
            LoadStyles(styles);

            var childInitializationTasks = new List<Task>();

            for (var i = 0; i < childCount; i++)
            {
                var child = hierarchy.ElementAt(i);
                
                if (child is UIComponent component)
                    childInitializationTasks.Add(component.InitializationTask);
            }

            await Task.WhenAll(childInitializationTasks);

            PostHierarchySetupProfilerMarker.Begin();

            ApplyEffects();
            PopulateQueryFields();
            RegisterEventInterfaceCallbacks();

            PostHierarchySetupProfilerMarker.End();

            OnInit();

            Initialized = true;
            _initCompletionSource.SetResult(this);
        }

        private void RegisterEventInterfaceCallbacks()
        {
            if (this is IOnAttachToPanel onAttachToPanel)
                RegisterCallback<AttachToPanelEvent>(onAttachToPanel.OnAttachToPanel);

            if (this is IOnDetachFromPanel onDetachFromPanel)
                RegisterCallback<DetachFromPanelEvent>(onDetachFromPanel.OnDetachFromPanel);

            if (this is IOnGeometryChanged onGeometryChanged)
                RegisterCallback<GeometryChangedEvent>(onGeometryChanged.OnGeometryChanged);

            if (this is IOnMouseEnter onMouseEnter)
                RegisterCallback<MouseEnterEvent>(onMouseEnter.OnMouseEnter);

            if (this is IOnMouseLeave onMouseLeave)
                RegisterCallback<MouseLeaveEvent>(onMouseLeave.OnMouseLeave);

#if UNITY_2020_3_OR_NEWER
            if (this is IOnClick onClick)
                RegisterCallback<ClickEvent>(onClick.OnClick);
#endif
#if UNITY_2021_3_OR_NEWER
            if (this is IOnNavigationMove onNavigationMove)
                RegisterCallback<NavigationMoveEvent>(onNavigationMove.OnNavigationMove);
#endif
        }

        private void LoadLayout([CanBeNull] VisualTreeAsset layoutAsset)
        {
            if (layoutAsset != null)
                layoutAsset.CloneTree(this);
        }

        private void LoadStyles(IList<StyleSheet> styles)
        {
            var styleSheetCount = styles.Count;

            for (var i = 0; i < styleSheetCount; i++)
                styleSheets.Add(styles[i]);
        }

        /// <summary>
        /// Called when all assets have been loaded and fields populated.
        /// </summary>
        /// <remarks>
        /// A bare component without assets will be initialized synchronously.
        /// In this case, this method will be called before the constructor returns.
        /// </remarks>
        public virtual void OnInit() {}

        /// <returns>A Task which resolves when the component has initialized</returns>
        [Obsolete("Use InitializationTask instead.")]
        public Task<UIComponent> WaitForInitialization()
        {
            return _initCompletionSource.Task;
        }

        /// <returns>An enumerator which yields when the component has initialized</returns>
        public IEnumerator WaitForInitializationEnumerator()
        {
            yield return _initCompletionSource.Task.AsEnumerator();
        }

        /// <summary>
        /// Returns the component's type's name.
        /// </summary>
        /// <returns>Type name</returns>
        public string GetTypeName()
        {
            return _componentType.Name;
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

        protected virtual Task<VisualTreeAsset> StartLayoutLoad()
        {
            return Task.FromResult<VisualTreeAsset>(null);
        }

        private Task<VisualTreeAsset> GetLayout()
        {
            return StartLayoutLoad();
        }
        
        protected readonly struct StyleSheetLoadTuple
        {
            public readonly string Path;
            public readonly StyleSheet StyleSheet;
            
            public StyleSheetLoadTuple(string path, StyleSheet styleSheet)
            {
                Path = path;
                StyleSheet = styleSheet;
            }
        }

        protected virtual Task<StyleSheetLoadTuple>[] StartStyleSheetLoad()
        {
            return Array.Empty<Task<StyleSheetLoadTuple>>();
        }

        private async Task<List<StyleSheet>> GetStyleSheets()
        {
            var styleSheetLoadTasks =
                StartStyleSheetLoad();

            await Task.WhenAll(styleSheetLoadTasks);

            var loadedStyleSheets = new List<StyleSheet>(styleSheetLoadTasks.Length);

            foreach (var loadTask in styleSheetLoadTasks)
            {
                var styleSheet = loadTask.Result.StyleSheet;

                if (styleSheet == null)
                {
                    Logger.LogError($"Could not find stylesheet {loadTask.Result.Path}", this);
                    continue;
                }

                loadedStyleSheets.Add(styleSheet);
            }

            return loadedStyleSheets;
        }

        private void ApplyEffects()
        {
            var effectAttributes = CacheDictionary[_componentType].EffectAttributes;
            var effectAttributeCount = effectAttributes.Count;

            for (var i = 0; i < effectAttributeCount; i++)
                effectAttributes[i].Apply(this);
        }

        protected virtual void PopulateQueryFields() {}

        protected virtual void PopulateProvideFields()
        {
            var fieldCache = CacheDictionary[_componentType].FieldCache;
            var provideAttributeDictionary = fieldCache.ProvideAttributes;

            foreach (var fieldInfo in provideAttributeDictionary.Keys)
            {
                var fieldType = fieldInfo.FieldType;

                if (provideAttributeDictionary[fieldInfo].CastFrom != null)
                    fieldType = provideAttributeDictionary[fieldInfo].CastFrom;

                object value;

                try
                {
                    value = _dependencyInjector.Provide(fieldType);

                    if (provideAttributeDictionary[fieldInfo].CastFrom != null)
                        value = Convert.ChangeType(value, fieldInfo.FieldType);
                }
                catch (MissingProviderException)
                {
                    Logger.LogError($"Could not provide {fieldInfo.FieldType.Name} to {fieldInfo.Name}", this);
                    continue;
                }
                catch (InvalidCastException)
                {
                    Logger.LogError($"Could not cast {fieldType.Name} to {fieldInfo.FieldType.Name}", this);
                    continue;
                }

                fieldInfo.SetValue(this, value);
            }
        }
    }
}
