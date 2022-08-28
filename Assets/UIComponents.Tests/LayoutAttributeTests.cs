using System.Collections;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class LayoutAttributeTests
    {
        [Layout("Assets/MyAsset.uxml")]
        private class UIComponentWithLayout : UIComponent {}
        
        private class InheritedComponentWithoutAttribute : UIComponentWithLayout {}
        
        [Layout("Assets/MyOtherAsset.uxml")]
        private class InheritedComponentWithAttribute : UIComponentWithLayout {}
        
        [Layout("Assets/MissingAsset.uxml")]
        private class UIComponentWithNullLayout : UIComponent {}

        private TestBed _testBed;
        private IAssetResolver _mockResolver;

        [SetUp]
        public void SetUp()
        {
            _mockResolver = MockUtilities.CreateMockResolver();
            _mockResolver.LoadAsset<VisualTreeAsset>("Assets/MissingAsset.uxml")
                .ReturnsNull();

            _testBed = TestBed.Create()
                .WithSingleton(_mockResolver)
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            _mockResolver.ClearReceivedCalls();
        }

        [UnityTest]
        public IEnumerator Given_Layout_Is_Loaded()
        {
            var component = _testBed.CreateComponent<UIComponentWithLayout>();
            yield return component.WaitForInitializationEnumerator();
            _mockResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Superclass_Layout_Is_Loaded_If_It_Is_Not_Overridden()
        {
            var component = _testBed.CreateComponent<InheritedComponentWithoutAttribute>();
            yield return component.WaitForInitializationEnumerator();
            _mockResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Superclass_Layout_Is_Not_Loaded_If_Overridden()
        {
            var component = _testBed.CreateComponent<InheritedComponentWithAttribute>();
            yield return component.WaitForInitializationEnumerator();
            _mockResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyOtherAsset.uxml");
            _mockResolver.DidNotReceive().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [Test]
        public void Null_Layout_Is_Handled()
        {
            Assert.DoesNotThrow(() => _testBed.CreateComponent<UIComponentWithNullLayout>());
        }
    }
}
