﻿using System.Collections;
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
    public class UIComponentTests
    {
        [TestFixture]
        public class Initialization
        {
            private class MockAssetResolver : IAssetResolver
            {
                public readonly Dictionary<string, object> TaskCompletionSources =
                    new Dictionary<string, object>();

                public Task<T> LoadAsset<T>(string assetPath) where T : Object
                {
                    var source = new TaskCompletionSource<T>();
                    TaskCompletionSources.Add(assetPath, source);
                    return source.Task;
                }

                public void CompleteLoad<T>(string assetPath) where T : ScriptableObject
                {
                    var source = (TaskCompletionSource<T>) TaskCompletionSources[assetPath];
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
            private class TestComponent : UIComponent {}

            private TestBed _testBed;
            private MockAssetResolver _mockAssetResolver;

            [SetUp]
            public void SetUp()
            {
                _mockAssetResolver = new MockAssetResolver();
                _testBed = TestBed.Create()
                    .WithSingleton<IAssetResolver>(_mockAssetResolver)
                    .Build();
            }

            [Test]
            public void Sets_The_Initialized_Value()
            {
                var component = _testBed.CreateComponent<TestComponent>();
                Assert.That(component.Initialized, Is.False);

                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");
                
                Assert.That(component.Initialized, Is.True);
            }

            [Test]
            public void Completes_Initialization_Task()
            {
                var component = _testBed.CreateComponent<TestComponent>();
                
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
                var component = _testBed.CreateComponent<TestComponent>();
                
                Assert.That(component.Initialized, Is.False);

                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");

                yield return component.WaitForInitializationEnumerator();
                
                Assert.That(component.Initialized, Is.True);
            }
            
            [UnityTest]
            public IEnumerator Allows_Waiting_For_Initialization_With_Obsolete_Method()
            {
                var component = _testBed.CreateComponent<TestComponent>();
                
                Assert.That(component.Initialized, Is.False);
                
                _mockAssetResolver.CompleteLoad<VisualTreeAsset>("Layout");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet1");
                _mockAssetResolver.CompleteLoad<StyleSheet>("Stylesheet2");

                yield return component.WaitForInitialization().AsEnumerator();
                
                Assert.That(component.Initialized, Is.True);
            }
            
            private class BareTestComponent : UIComponent {}

            [Test]
            public void Bare_Component_Initializes_Synchronously()
            {
                var component = new BareTestComponent();
                Assert.That(component.Initialized, Is.True);
            }
        }
        
        [TestFixture]
        public class GetTypeName
        {
            private class TestComponent : UIComponent {}

            [Test]
            public void ShouldReturnTypeName()
            {
                var component = new TestComponent();
                Assert.That(nameof(TestComponent), Is.EqualTo(component.GetTypeName()));
            }
        }
    }
}
