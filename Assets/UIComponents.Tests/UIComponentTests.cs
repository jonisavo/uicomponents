using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
#pragma warning disable CS4014

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
                component.Initialize();
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
                component.Initialize();
                
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
                component.Initialize();
                
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
                component.Initialize();

                var childTestBed = new TestBed<ChildComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var firstChild = childTestBed.CreateComponent();
                firstChild.Initialize();
                var secondChild = childTestBed.CreateComponent();
                secondChild.Initialize();

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
                component.Initialize();

                var childTestBed = new TestBed<ChildComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var firstChild = childTestBed.CreateComponent();
                firstChild.Initialize();
                var secondChild = childTestBed.CreateComponent();
                secondChild.Initialize();

                var nestedChildTestBed = new TestBed<NestedChildComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var nestedChild = nestedChildTestBed.CreateComponent();
                nestedChild.Initialize();
                
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
                
                component.Initialize();
                
                Assert.That(component.Initialized, Is.True);
            }

            private partial class BareInitCounterComponent : UIComponent
            {
                public int InitCount { get; private set; }
                
                public override void OnInit()
                {
                    InitCount++;
                }
            }

            [Test]
            public void Synchronous_Initialization_Happens_Once()
            {
                var component = new BareInitCounterComponent();
                
                component.Initialize();
                component.Initialize();
                component.Initialize();
                
                Assert.That(component.InitCount, Is.EqualTo(1));
            }
            
            [Layout("Layout")]
            private partial class InitCounterComponent : BareInitCounterComponent {}
            
            [UnityTest]
            public IEnumerator Asynchronous_Initialization_Happens_Once()
            {
                var testBed = new TestBed<InitCounterComponent>()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver);

                var component = testBed.CreateComponent();
                
                component.Initialize();
                component.Initialize();
                component.Initialize();
                
                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                
                yield return component.WaitForInitializationEnumerator();
                
                Assert.That(component.InitCount, Is.EqualTo(1));
            }
        }
    }
}
