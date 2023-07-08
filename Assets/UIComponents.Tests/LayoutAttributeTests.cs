using System.Collections;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UIComponents.Internal;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class LayoutAttributeTests
    {
        [Layout("Assets/MyAsset.uxml")]
        private partial class UIComponentWithLayout : UIComponent {}
        
        private partial class InheritedComponentWithoutAttribute : UIComponentWithLayout {}
        
        [Layout("Assets/MyOtherAsset.uxml")]
        private partial class InheritedComponentWithAttribute : UIComponentWithLayout {}
        
        [Layout("Assets/MissingAsset.uxml")]
        private partial class UIComponentWithNullLayout : UIComponent {}

        private IAssetResolver _mockResolver;

        [SetUp]
        public void SetUp()
        {
            _mockResolver = MockUtilities.CreateMockResolver();
            _mockResolver.LoadAsset<VisualTreeAsset>("Assets/MissingAsset.uxml")
                .ReturnsNull();
        }

        [TearDown]
        public void TearDown()
        {
            _mockResolver.ClearReceivedCalls();
        }

        [UnityTest]
        public IEnumerator Given_Layout_Is_Loaded()
        {
            var testBed = new TestBed<UIComponentWithLayout>().WithSingleton(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Superclass_Layout_Is_Loaded_If_It_Is_Not_Overridden()
        {
            var testBed = new TestBed<InheritedComponentWithoutAttribute>().WithSingleton(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Superclass_Layout_Is_Not_Loaded_If_Overridden()
        {
            var testBed = new TestBed<InheritedComponentWithAttribute>().WithSingleton(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyOtherAsset.uxml");
            _mockResolver.DidNotReceive().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Null_Layout_Is_Handled()
        {
            var testBed = new TestBed<UIComponentWithNullLayout>().WithSingleton(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
        }
    }
}
