using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UIComponents.Internal;
using UIComponents.Testing;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class UIComponentTests
    {
        [TestFixture]
        public partial class Initialization
        {
            private class MockAssetResolver : IAssetResolver
            {
                private readonly Dictionary<string, object> _taskCompletionSources =
                    new Dictionary<string, object>();

                public Task<T> LoadAsset<T>(string assetPath) where T : Object
                {
                    if (!_taskCompletionSources.ContainsKey(assetPath))
                    {
                        var source = new TaskCompletionSource<T>();
                        _taskCompletionSources.Add(assetPath, source);
                    }

                    return ((TaskCompletionSource<T>) _taskCompletionSources[assetPath]).Task;
                }

                public void CompleteLoad<T>(string assetPath) where T : ScriptableObject
                {
                    var source = (TaskCompletionSource<T>) _taskCompletionSources[assetPath];
                    source.SetResult(ScriptableObject.CreateInstance<T>());
                }

                public Task<bool> AssetExists(string assetPath)
                {
                    return Task.FromResult(true);
                }
            }

            [Layout("Layout")]
            [Stylesheet("Stylesheet1")]
            [Stylesheet("Stylesheet2")]
            private partial class TestComponent : UIComponent {}

            private TestBed<TestComponent> _testComponentTestBed;
            private MockAssetResolver _mockAssetResolver;

            [SetUp]
            public void SetUp()
            {
                _mockAssetResolver = new MockAssetResolver();
                _testComponentTestBed = new TestBed<TestComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);
            }

            [Test]
            public void Sets_The_Initialized_Value()
            {
                var component = _testComponentTestBed.CreateComponent();
                Assert.That(component.Initialized, Is.False);

                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");
                
                Assert.That(component.Initialized, Is.True);
            }

            [Test]
            public void Completes_Initialization_Task()
            {
                var component = _testComponentTestBed.CreateComponent();
                
                Assert.That(component.InitializationTask.IsCompleted, Is.False);
                
                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");
                
                Assert.That(component.InitializationTask.IsCompleted, Is.True);
                Assert.That(component.InitializationTask.Result, Is.SameAs(component));
            }

            [UnityTest]
            public IEnumerator Allows_Waiting_For_Initialization()
            {
                var component = _testComponentTestBed.CreateComponent();
                
                Assert.That(component.Initialized, Is.False);

                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");

                yield return component.WaitForInitializationEnumerator();
                
                Assert.That(component.Initialized, Is.True);
            }

            [Layout("Child")]
            [Stylesheet("ChildStylesheet")]
            private partial class ChildComponent : UIComponent {}
            
            [Layout("NestedChild")]
            private partial class NestedChildComponent : UIComponent {}

            [Test]
            public void Does_Not_Initialize_If_Children_Are_Uninitialized()
            {
                var component = _testComponentTestBed.CreateComponent();

                var childTestBed = new TestBed<ChildComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var firstChild = childTestBed.CreateComponent();
                var secondChild = childTestBed.CreateComponent();

                component.Add(firstChild);
                component.Add(secondChild);
                
                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");

                Assert.That(component.Initialized, Is.False);
            }

            [Test]
            public void Initializes_When_Children_Are_Initialized()
            {
                var component = _testComponentTestBed.CreateComponent();

                var childTestBed = new TestBed<ChildComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var firstChild = childTestBed.CreateComponent();
                var secondChild = childTestBed.CreateComponent();

                var nestedChildTestBed = new TestBed<NestedChildComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var nestedChild = nestedChildTestBed.CreateComponent();
                
                firstChild.Add(nestedChild);

                component.Add(firstChild);
                component.Add(secondChild);
                
                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");

                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Child");
                _mockAssetResolver.CompleteLoad<StyleSheet>("ChildStylesheet");
                
                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("NestedChild");
                
                Assert.That(component.Initialized, Is.True);
            }
            
            private partial class BareTestComponent : UIComponent {}

            [Test]
            public void Bare_Component_Initializes_Synchronously()
            {
                var component = new BareTestComponent();
                Assert.That(component.Initialized, Is.True);
            }
        }
    }
}
