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

        [Layout]
        private partial class UIComponentWithConventionLayout : UIComponent {}

        private IAssetSource _mockSource;

        [SetUp]
        public void SetUp()
        {
            _mockSource = MockUtilities.CreateMockSource();
            _mockSource.LoadAsset<VisualTreeAsset>("Assets/MissingAsset.uxml")
                .ReturnsNull();
        }

        [TearDown]
        public void TearDown()
        {
            _mockSource.ClearReceivedCalls();
        }

        [UnityTest]
        public IEnumerator Given_Layout_Is_Loaded()
        {
            var testBed = new TestBed<UIComponentWithLayout>().WithSingleton(_mockSource);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockSource.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Superclass_Layout_Is_Loaded_If_It_Is_Not_Overridden()
        {
            var testBed = new TestBed<InheritedComponentWithoutAttribute>().WithSingleton(_mockSource);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockSource.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Superclass_Layout_Is_Not_Loaded_If_Overridden()
        {
            var testBed = new TestBed<InheritedComponentWithAttribute>().WithSingleton(_mockSource);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockSource.Received().LoadAsset<VisualTreeAsset>("Assets/MyOtherAsset.uxml");
            _mockSource.DidNotReceive().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [UnityTest]
        public IEnumerator Null_Layout_Is_Handled()
        {
            var testBed = new TestBed<UIComponentWithNullLayout>().WithSingleton(_mockSource);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
        }

        [UnityTest]
        public IEnumerator Convention_Layout_Uses_Class_Name()
        {
            var testBed = new TestBed<UIComponentWithConventionLayout>().WithSingleton(_mockSource);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();
            _mockSource.Received().LoadAsset<VisualTreeAsset>("UIComponentWithConventionLayout");
        }
    }
}
