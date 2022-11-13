using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIComponents.DependencyInjection;
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
    /// <seealso cref="DebugLogger"/>
    [Dependency(typeof(IAssetResolver), provide: typeof(ResourcesAssetResolver))]
    [Dependency(typeof(ILogger), provide: typeof(DebugLogger))]
    public abstract class UIComponent : VisualElement, IDependencyConsumer
    {
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
        /// The logger used by this UIComponent.
        /// Defaults to <see cref="DebugLogger"/>.
        /// </summary>
        protected readonly ILogger Logger;

        private readonly DependencyInjector _dependencyInjector;

        private readonly TaskCompletionSource<UIComponent> _initCompletionSource =
            new TaskCompletionSource<UIComponent>();

        private static readonly ProfilerMarker DependencySetupProfilerMarker =
            new ProfilerMarker("UIComponent.DependencySetup");

        /// <summary>
        /// UIComponent's constructor loads the configured layout and stylesheets.
        /// </summary>
        protected UIComponent()
        {
            DependencySetupProfilerMarker.Begin();

            DiContext.Current.RegisterConsumer(this);
            _dependencyInjector = DiContext.Current.GetInjector(GetType());
            AssetResolver = Provide<IAssetResolver>();
            Logger = Provide<ILogger>();
            UIC_PopulateProvideFields();

            DependencySetupProfilerMarker.End();

            Initialize();
        }

        private async void Initialize()
        {
            var layoutTask = UIC_StartLayoutLoad();
            var stylesTask = GetStyleSheets();

            await Task.WhenAll(layoutTask, stylesTask);

            var layoutAsset = layoutTask.Result;
            var styles = stylesTask.Result;

            if (layoutAsset != null)
                layoutAsset.CloneTree(this);
            
            for (var i = 0; i < styles.Count; i++)
                styleSheets.Add(styles[i]);

            var childInitializationTasks = new List<Task>();

            for (var i = 0; i < childCount; i++)
            {
                var child = hierarchy.ElementAt(i);
                
                if (child is UIComponent component)
                    childInitializationTasks.Add(component.InitializationTask);
            }

            await Task.WhenAll(childInitializationTasks);

            UIC_ApplyEffects();
            UIC_PopulateQueryFields();
            RegisterEventInterfaceCallbacks();

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
            
            if (this is IOnClick onClick)
                RegisterCallback<ClickEvent>(onClick.OnClick);
            
#if UNITY_2021_3_OR_NEWER
            if (this is IOnNavigationMove onNavigationMove)
                RegisterCallback<NavigationMoveEvent>(onNavigationMove.OnNavigationMove);
#endif
        }

        private static readonly IDependency[] EmptyDependencyArray =
            Array.Empty<IDependency>();

        public virtual IEnumerable<IDependency> GetDependencies()
        {
            Debug.Log("UIComponents: GetDependencies for " + GetType().Name);
            return EmptyDependencyArray;
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
        
        protected virtual Task<VisualTreeAsset> UIC_StartLayoutLoad()
        {
            return Task.FromResult<VisualTreeAsset>(null);
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

        protected virtual Task<StyleSheetLoadTuple>[] UIC_StartStyleSheetLoad()
        {
            return Array.Empty<Task<StyleSheetLoadTuple>>();
        }

        private async Task<List<StyleSheet>> GetStyleSheets()
        {
            var styleSheetLoadTasks =
                UIC_StartStyleSheetLoad();

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

        protected virtual void UIC_ApplyEffects() {}

        protected virtual void UIC_PopulateQueryFields() {}

        protected virtual void UIC_PopulateProvideFields() {}
    }
}
