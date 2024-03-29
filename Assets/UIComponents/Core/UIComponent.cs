﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using UIComponents.DependencyInjection;
using UIComponents.Internal;
using Unity.Profiling;
using UnityEngine.TestTools;
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
        private bool _initializationOngoing;

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
            // ReSharper disable once VirtualMemberCallInConstructor
            UIC_PopulateProvideFields();

            DependencySetupProfilerMarker.End();

            RegisterCallback<AttachToPanelEvent>(OnFirstAttachToPanel);
        }
        
        ~UIComponent()
        {
            UnregisterCallback<AttachToPanelEvent>(OnFirstAttachToPanel);
            UIC_UnregisterEventCallbacks();
        }

        [ExcludeFromCodeCoverage] // Pragmas are shown as uncovered lines
        private void OnFirstAttachToPanel(AttachToPanelEvent evt)
        {
#pragma warning disable CS4014
            Initialize();
#pragma warning restore CS4014
            UnregisterCallback<AttachToPanelEvent>(OnFirstAttachToPanel);
        }

        /// <summary>
        /// Starts the initialization of the UIComponent. Resolves immediately if the UIComponent
        /// has already been initialized or if initialization is already ongoing.
        /// </summary>
        /// <remarks>
        /// This method is called automatically when the UIComponent is first attached to a panel.
        /// It can also be called manually to force initialization.
        /// </remarks>
        public async Task Initialize()
        {
            if (Initialized || _initializationOngoing)
                return;
            
            _initializationOngoing = true;
            
            var layoutTask = UIC_StartLayoutLoad();
            var stylesTask = GetStyleSheets();

            await Task.WhenAll(layoutTask, stylesTask);

            var layoutAsset = layoutTask.Result;
            var styleTuples = stylesTask.Result;

            if (layoutAsset != null)
                layoutAsset.CloneTree(this);
            
            for (var i = 0; i < styleTuples.Length; i++)
            {
                var tuple = styleTuples[i];

                if (tuple.StyleSheet == null)
                {
                    Logger.LogError($"Could not find stylesheet {tuple.Path}", this);
                    continue;
                }
                
                styleSheets.Add(tuple.StyleSheet);
            }

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
            UIC_RegisterEventCallbacks();

            OnInit();

            Initialized = true;
            _initializationOngoing = false;
            _initCompletionSource.SetResult(this);
        }

        protected virtual void UIC_RegisterEventCallbacks() {}

        protected virtual void UIC_UnregisterEventCallbacks() {}

        private static readonly IDependency[] EmptyDependencyArray =
            Array.Empty<IDependency>();

        [ExcludeFromCoverage]
        public virtual IEnumerable<IDependency> GetDependencies()
        {
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
        
        private static readonly Task<VisualTreeAsset> NullLayoutTask =
            Task.FromResult<VisualTreeAsset>(null);

        protected virtual Task<VisualTreeAsset> UIC_StartLayoutLoad()
        {
            return NullLayoutTask;
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

        private Task<StyleSheetLoadTuple[]> GetStyleSheets()
        {
            var styleSheetLoadTasks =
                UIC_StartStyleSheetLoad();
            
            if (styleSheetLoadTasks.Length == 0)
                return Task.FromResult(Array.Empty<StyleSheetLoadTuple>());

            return Task.WhenAll(styleSheetLoadTasks);
        }

        protected virtual void UIC_ApplyEffects() {}

        protected virtual void UIC_PopulateQueryFields() {}

        protected virtual void UIC_PopulateProvideFields() {}
    }
}
